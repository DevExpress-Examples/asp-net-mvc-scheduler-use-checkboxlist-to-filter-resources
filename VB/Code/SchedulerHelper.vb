Imports System
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraScheduler
Imports System.Runtime.CompilerServices

Namespace SchedulerFilterResourcesDataLevelMvc.Code

    Public Module SchedulerHelper

        Private settingsField As SchedulerSettings

        Public ReadOnly Property Settings As SchedulerSettings
            Get
                If settingsField Is Nothing Then settingsField = CreateSchedulerSettings(Nothing)
                Return settingsField
            End Get
        End Property

        <Extension()>
        Public Function CreateSchedulerSettings(ByVal htmlHelper As HtmlHelper) As SchedulerSettings
            Dim settings As SchedulerSettings = New SchedulerSettings()
            settings.Name = "scheduler"
            settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "SchedulerPartial"}
            settings.EditAppointmentRouteValues = New With {.Controller = "Home", .Action = "EditAppointment"}
            settings.Storage.Appointments.Assign(DefaultAppointmentStorage)
            settings.Storage.Resources.Assign(DefaultResourceStorage)
            settings.GroupType = SchedulerGroupType.Resource
            settings.ActiveViewType = SchedulerViewType.Timeline
            settings.Views.TimelineView.Styles.TimelineCellBody.Height = Web.UI.WebControls.Unit.Pixel(80)
            settings.ClientSideEvents.BeginCallback = "OnBeginCallback"
            settings.Storage.Appointments.ResourceSharing = True
            settings.Start = New DateTime(2008, 7, 11)
            Return settings
        End Function

        Private defaultAppointmentStorageField As MVCxAppointmentStorage

        Public ReadOnly Property DefaultAppointmentStorage As MVCxAppointmentStorage
            Get
                If defaultAppointmentStorageField Is Nothing Then defaultAppointmentStorageField = CreateDefaultAppointmentStorage()
                Return defaultAppointmentStorageField
            End Get
        End Property

        Private Function CreateDefaultAppointmentStorage() As MVCxAppointmentStorage
            Dim appointmentStorage As MVCxAppointmentStorage = New MVCxAppointmentStorage()
            appointmentStorage.Mappings.AppointmentId = "ID"
            appointmentStorage.Mappings.Start = "StartTime"
            appointmentStorage.Mappings.End = "EndTime"
            appointmentStorage.Mappings.Subject = "Subject"
            appointmentStorage.Mappings.Description = "Description"
            appointmentStorage.Mappings.Location = "Location"
            appointmentStorage.Mappings.AllDay = "AllDay"
            appointmentStorage.Mappings.Type = "EventType"
            appointmentStorage.Mappings.RecurrenceInfo = "RecurrenceInfo"
            appointmentStorage.Mappings.ReminderInfo = "ReminderInfo"
            appointmentStorage.Mappings.Label = "Label"
            appointmentStorage.Mappings.Status = "Status"
            appointmentStorage.Mappings.ResourceId = "CarId"
            Return appointmentStorage
        End Function

        Private defaultResourceStorageField As MVCxResourceStorage

        Public ReadOnly Property DefaultResourceStorage As MVCxResourceStorage
            Get
                If defaultResourceStorageField Is Nothing Then defaultResourceStorageField = CreateDefaultResourceStorage()
                Return defaultResourceStorageField
            End Get
        End Property

        Private Function CreateDefaultResourceStorage() As MVCxResourceStorage
            Dim resourceStorage As MVCxResourceStorage = New MVCxResourceStorage()
            resourceStorage.Mappings.ResourceId = "ID"
            resourceStorage.Mappings.Caption = "Model"
            Return resourceStorage
        End Function
    End Module
End Namespace
