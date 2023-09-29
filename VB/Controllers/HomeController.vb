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
            Call UpdateAppointment()
            Return PartialView("SchedulerPartial", SchedulerDataHelper.GetDataObject(GetSelectedResourceIds()))
        End Function

        Private Function GetSelectedResourceIds() As List(Of Integer)
            Dim request As String = If(Not Equals(Me.Request.Params("SelectedResources"), Nothing), Me.Request.Params("SelectedResources"), String.Empty)
            Return If(Not Equals(request, String.Empty), request.Split(","c).[Select](Function(n) Convert.ToInt32(n)).ToList(Of Integer)(), New List(Of Integer)())
        End Function

        Private Shared Sub UpdateAppointment()
            Dim insertedAppt As CarScheduling = SchedulerExtension.GetAppointmentToInsert(Of CarScheduling)(Settings, SchedulerDataHelper.GetAppointments(), SchedulerDataHelper.GetResources())
            SchedulerDataHelper.InsertAppointment(insertedAppt)
            Dim updatedAppt As CarScheduling() = SchedulerExtension.GetAppointmentsToUpdate(Of CarScheduling)(Settings, SchedulerDataHelper.GetAppointments(), SchedulerDataHelper.GetResources())
            For Each appt In updatedAppt
                SchedulerDataHelper.UpdateAppointment(appt)
            Next

            Dim removedAppt As CarScheduling() = SchedulerExtension.GetAppointmentsToRemove(Of CarScheduling)(Settings, SchedulerDataHelper.GetAppointments(), SchedulerDataHelper.GetResources())
            For Each appt In removedAppt
                SchedulerDataHelper.RemoveAppointment(appt)
            Next
        End Sub
    End Class
End Namespace
