namespace ProSoft.EasyLog.Models
{
    /// <summary>
    /// Job-level events (not tied to a file transfer).
    /// Used by EasySave v2.0+ for business software detection.
    /// </summary>
    public enum JobEventType
    {
        Refused,
        Interrupted
    }
}
