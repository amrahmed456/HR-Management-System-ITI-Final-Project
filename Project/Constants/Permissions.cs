namespace FinalProject.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));

            return allPermissions;
        }

        public static class Employee
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Edit = "Permissions.Employee.Edit";
            public const string Delete = "Permissions.Employee.Delete";
        }
        public static class EmployeeAttendance
        {
            public const string View = "Permissions.EmployeeAttendance.View";
            public const string Create = "Permissions.EmployeeAttendance.Create";
            public const string Edit = "Permissions.EmployeeAttendance.Edit";
            public const string Delete = "Permissions.EmployeeAttendance.Delete";
        }
        public static class GeneralSettings
        {
            public const string View = "Permissions.GeneralSettings.View";
            public const string Create = "Permissions.GeneralSettings.Create";
            public const string Edit = "Permissions.GeneralSettings.Edit";
            public const string Delete = "Permissions.GeneralSettings.Delete";
        }
        public static class Department
        {
            public const string View = "Permissions.Department.View";
            public const string Create = "Permissions.Department.Create";
            public const string Edit = "Permissions.Department.Edit";
            public const string Delete = "Permissions.Department.Delete";
        }

        public static class Reports
        {
            public const string View = "Permissions.Reports.View";
            public const string Create = "Permissions.Reports.Create";
            public const string Edit = "Permissions.Reports.Edit";
            public const string Delete = "Permissions.Reports.Delete";
        }

        public static class Staff
        {
            public const string View = "Permissions.Staff.View";
            public const string Create = "Permissions.Staff.Create";
            public const string Edit = "Permissions.Staff.Edit";
            public const string Delete = "Permissions.Staff.Delete";
        }

        public static class Vacations
        {
            public const string View = "Permissions.Vacations.View";
            public const string Create = "Permissions.Vacations.Create";
            public const string Edit = "Permissions.Vacations.Edit";
            public const string Delete = "Permissions.Vacations.Delete";
        }
    }
}
