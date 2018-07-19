namespace Seqrus.Web.Helpers
{
    public class ComplianceSettings
    {
        public bool SecurityMisconfiguration { get; set; }
        public bool CrossSiteScripting { get; set; }
        public bool SensitiveDataExposure { get; set; }
        public bool InsufficientLoggingAndMonitoring { get; set; }
    }
}
