namespace Apps.Runtime.Common
{
    public enum SyncType
    {
        SpectrumBeat,       // sync tile with beats based on amplitude threshold of spectrum data.
        SpectrumRythmVocal, // sync tile with vocals & rhythms based on the high frequencies of spectrum data.
    }

    public enum PointRank
    {
        None,
        Good,
        Great,
        Perfect,
    }
}