namespace ProSoft.EasyLog.Models
{
    /// Job-level events (not tied to a file transfer).
    /// Used by EasySave v2.0+ for business software detection
    public enum JobEventType
    {
        Refused,
        Interrupted
    }
}
