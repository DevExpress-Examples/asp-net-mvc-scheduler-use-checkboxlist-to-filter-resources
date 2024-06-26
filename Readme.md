<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128553367/14.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4717)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Scheduler for ASP.NET MVC - How to use CheckBoxList to filter resources

This is a counterpart of the [How to filter resources in ASPxScheduler via ASPxListBox](https://github.com/DevExpress-Examples/how-to-filter-resources-in-aspxscheduler-via-aspxlistbox-e3783) code example but for ASP.NET MVC platform.

## Overview
    
As a starting point, we are using the data-bound Scheduler in the partial view (see Note section in the [Callback-Based Functionality](https://docs.devexpress.com/AspNetMvc/9052/common-features/callback-based-functionality) help article). You can find a similar logic in [Lesson 2 - Implement Insert-Update-Delete Appointment Functionality](https://docs.devexpress.com/AspNetMvc/11567/components/scheduler/get-started/lesson-2-implement-the-insert-update-delete-appointment-functionality). But our code is more extensible and reliable for the following reason.

We use the **SchedulerHelper** class to initialize Scheduler settings for both view and controller. This allows us to implement a reliable solution according to the [Lesson 3 - Use Scheduler in Complex Views](https://docs.devexpress.com/AspNetMvc/11629/components/scheduler/get-started/lesson-3-use-scheduler-in-complex-views). This is preferable implementation, which should operate correctly in any possible scenarios.

It is not necessary to isolate CheckBoxList in a partial view because it is not operating in callback mode. Thus, we place it in the main view:

```cs
@model SchedulerFilterResourcesDataLevelMvc.Models.SchedulerDataObject

<script type="text/javascript">
// <![CDATA[
    function OnSelectedIndexChanged(s, e) {
        scheduler.PerformCallback();
    }
    function OnBeginCallback(s, e) {
        e.customArgs['SelectedResources'] = cbResources.GetSelectedValues().join(',');
    }
// ]]>
</script>

<table>
    <tr>
        <td valign="top">
            @Html.DevExpress().CheckBoxList(settings => {
                settings.Name = "cbResources";
                settings.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                settings.Properties.ClientSideEvents.SelectedIndexChanged = "OnSelectedIndexChanged";
                settings.Properties.ValueField = "ID";
                settings.Properties.TextField = "Model";
            }).BindList(Model.Resources).GetHtml()
        </td>
        <td>
            @Html.Partial("SchedulerPartial", Model)
        </td>
    </tr>
</table>
```

The `OnBeginCallback` function name is assigned to the `settings.ClientSideEvents.BeginCallback` attribute of the Scheduler settings initialized in the **SchedulerHelper** class. Thus, this function is called before callback occurs. We pass parameters to the corresponding controller's action in this function as described in the[Passing Values to Controller Action Through Callbacks](https://docs.devexpress.com/AspNetMvc/9941/common-features/callback-based-functionality/passing-values-to-a-controller-action-through-callbacks) help topic. This action is defined as follows:

```cs
public ActionResult SchedulerPartial() {
    return PartialView("SchedulerPartial", SchedulerDataHelper.GetDataObject(GetSelectedResourceIds()));
}
...
List<int> GetSelectedResourceIds() {
    string request = (Request.Params["SelectedResources"] != null) ? (Request.Params["SelectedResources"]) : string.Empty;
    return (request != string.Empty) ? request.Split(',').Select(n => Convert.ToInt32(n)).ToList<int>() : new List<int>();
}
```

The `SchedulerDataHelper.GetDataObject()` method is implemented so that the returned object resources are filtered by a list of Ids passed to this method. Note that we use this method in the `EditAppointment` action either.

## Files to Review

* [SchedulerHelper.cs](./CS/Code/SchedulerHelper.cs) (VB: [SchedulerHelper.vb](./VB/Code/SchedulerHelper.vb))
* [HomeController.cs](./CS/Controllers/HomeController.cs) (VB: [HomeController.vb](./VB/Controllers/HomeController.vb))
* [Scheduling.cs](./CS/Models/Scheduling.cs) (VB: [Scheduling.vb](./VB/Models/Scheduling.vb))
* [Index.cshtml](./CS/Views/Home/Index.cshtml)
* [SchedulerPartial.cshtml](./CS/Views/Home/SchedulerPartial.cshtml)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-mvc-scheduler-use-checkboxlist-to-filter-resources&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-mvc-scheduler-use-checkboxlist-to-filter-resources&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
