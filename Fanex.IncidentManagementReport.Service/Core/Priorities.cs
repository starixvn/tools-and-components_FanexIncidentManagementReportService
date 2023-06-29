namespace Fanex.IncidentManagementReport.Service.Core
{
    using System.ComponentModel;

    public enum Priorities
    {
        [Description("None")]
        None = 0,

        [Description("P1")]
        P1 = 1,

        [Description("P2")]
        P2 = 2,

        [Description("P3")]
        P3 = 3
    }
}