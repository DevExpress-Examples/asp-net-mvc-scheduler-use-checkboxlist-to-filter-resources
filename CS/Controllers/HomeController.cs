using System;
using System.Linq;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SchedulerFilterResourcesDataLevelMvc.Code;
using SchedulerFilterResourcesDataLevelMvc.Models;
using System.Collections.Generic;

namespace SchedulerFilterResourcesDataLevelMvc {
    public class HomeController: Controller {
        public ActionResult Index() {
            return View(SchedulerDataHelper.DataObject);
        }

        public ActionResult SchedulerPartial() {
            return PartialView("SchedulerPartial", SchedulerDataHelper.GetDataObject(GetSelectedResourceIds()));
        }

        public ActionResult EditAppointment() {
            UpdateAppointment();
            return PartialView("SchedulerPartial", SchedulerDataHelper.GetDataObject(GetSelectedResourceIds()));
        }

        List<int> GetSelectedResourceIds() {
            string request = (Request.Params["SelectedResources"] != null) ? (Request.Params["SelectedResources"]) : string.Empty;
            return (request != string.Empty) ? request.Split(',').Select(n => Convert.ToInt32(n)).ToList<int>() : new List<int>();
        }

        static void UpdateAppointment() {
            CarScheduling insertedAppt = SchedulerExtension.GetAppointmentToInsert<CarScheduling>(
                SchedulerHelper.Settings,
                SchedulerDataHelper.GetAppointments(),
                SchedulerDataHelper.GetResources()
            );
            SchedulerDataHelper.InsertAppointment(insertedAppt);

            CarScheduling[] updatedAppt = SchedulerExtension.GetAppointmentsToUpdate<CarScheduling>(
                SchedulerHelper.Settings,
                SchedulerDataHelper.GetAppointments(),
                SchedulerDataHelper.GetResources()
            );
            foreach (var appt in updatedAppt) {
                SchedulerDataHelper.UpdateAppointment(appt);
            }

            CarScheduling[] removedAppt = SchedulerExtension.GetAppointmentsToRemove<CarScheduling>(
                SchedulerHelper.Settings,
                SchedulerDataHelper.GetAppointments(),
                SchedulerDataHelper.GetResources()
            );
            foreach (var appt in removedAppt) {
                SchedulerDataHelper.RemoveAppointment(appt);
            }
        }
    }
}