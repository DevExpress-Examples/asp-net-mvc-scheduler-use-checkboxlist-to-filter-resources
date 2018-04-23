Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc
Imports SchedulerFilterResourcesDataLevelMvc.Code
Imports SchedulerFilterResourcesDataLevelMvc.Models
Imports System.Collections.Generic

Namespace SchedulerFilterResourcesDataLevelMvc
	Public Class HomeController
		Inherits Controller
		Public Function Index() As ActionResult
			Return View(SchedulerDataHelper.DataObject)
		End Function

		Public Function SchedulerPartial() As ActionResult
			Return PartialView("SchedulerPartial", SchedulerDataHelper.GetDataObject(GetSelectedResourceIds()))
		End Function

		Public Function EditAppointment() As ActionResult
			UpdateAppointment()
			Return PartialView("SchedulerPartial", SchedulerDataHelper.GetDataObject(GetSelectedResourceIds()))
		End Function

		Private Function GetSelectedResourceIds() As List(Of Integer)
            Dim req As String = IIf(Request.Params("SelectedResources") IsNot Nothing, Request.Params("SelectedResources"), String.Empty).ToString()

            If req Is String.Empty Then
                Return New List(Of Integer)
            End If

            Return CType(IIf(req <> String.Empty, req.Split(","c).Select(Function(n) Convert.ToInt32(n)).ToList(), New List(Of Integer)()), List(Of Integer))
        End Function

		Private Shared Sub UpdateAppointment()
			Dim insertedAppt As CarScheduling = SchedulerExtension.GetAppointmentToInsert(Of CarScheduling)(SchedulerHelper.Settings, SchedulerDataHelper.GetAppointments(), SchedulerDataHelper.GetResources())
			SchedulerDataHelper.InsertAppointment(insertedAppt)

			Dim updatedAppt() As CarScheduling = SchedulerExtension.GetAppointmentsToUpdate(Of CarScheduling)(SchedulerHelper.Settings, SchedulerDataHelper.GetAppointments(), SchedulerDataHelper.GetResources())
			For Each appt In updatedAppt
				SchedulerDataHelper.UpdateAppointment(appt)
			Next appt

			Dim removedAppt() As CarScheduling = SchedulerExtension.GetAppointmentsToRemove(Of CarScheduling)(SchedulerHelper.Settings, SchedulerDataHelper.GetAppointments(), SchedulerDataHelper.GetResources())
			For Each appt In removedAppt
				SchedulerDataHelper.RemoveAppointment(appt)
			Next appt
		End Sub
	End Class
End Namespace