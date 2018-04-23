# How to filter resources in Scheduler via CheckBoxList


<p>This is a counterpart of the <a href="https://www.devexpress.com/Support/Center/p/E3783">How to filter resources in ASPxScheduler via ASPxListBox</a> code example but for ASP.NET MVC platform. The implementation for this platform is quite different. As a starting point, we are using the data-bound Scheduler in the partial view (see Note section in the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument9052">Using Callbacks</a> help article). You can find a similar logic in <a href="http://documentation.devexpress.com/#AspNet/CustomDocument11567">Lesson 2 - Implement Insert-Update-Delete Appointment Functionality</a>. But our code is more extensible and reliable for the following reason.</p><p></p><p>We use the <strong>SchedulerHelper </strong>class to initialize Scheduler settings for both view and controller. This allows us to implement a reliable solution according to the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument11629">Lesson 3 - Use Scheduler in Complex Views</a>. This is preferable implementation, which should operate correctly in any possible scenarios.</p><p></p><p>It is not necessary to isolate the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument10686">CheckBoxList</a> in a partial view because it is not operating in callback mode. Thus, we place it in the main view:</p><p></p>

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

<p></p><p>The <strong>OnBeginCallback </strong>function name is assigned to the 'settings.ClientSideEvents.BeginCallback' attribute of the Scheduler settings initialized in the <strong>SchedulerHelper </strong>class. Thus, this function is called before callback occurs. We pass parameters to the corresponding controller's action in this function as described in the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument9941">Passing Values to Controller Action Through Callbacks</a> help section. This action is defined as follows:</p><p></p>

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

<p></p><p>The <strong>SchedulerDataHelper.GetDataObject()</strong> method is implemented so that the returned object resources are filtered by a list of Ids passed to this method. Note that we use this method in the <strong>EditAppointment </strong>action either.</p><p></p><p><strong>See Also:</strong></p><p><a href="https://www.devexpress.com/Support/Center/p/E4496">Scheduler - How to filter appointments by resources</a></p>

<br/>


