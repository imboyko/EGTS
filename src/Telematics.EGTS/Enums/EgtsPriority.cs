namespace Telematics.EGTS
{
    /// <summary>
    /// Приоритет обработки.
    /// </summary>
    public enum EgtsPriority : byte
    {
        /// <summary>
        /// Наивысший приоритет.
        /// </summary>
        Highest,

        /// <summary>
        /// Высокий приоритет.
        /// </summary>
        High,

        /// <summary>
        /// Средний приоритет.
        /// </summary>
        Normal,

        /// <summary>
        /// Низкий приоритет.
        /// </summary>
        Low
    }
}