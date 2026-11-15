namespace WinCAN.Core.FileFormats;

/// <summary>One entry of the open/save dialog filter list.</summary>
/// <param name="Name">Format name shown in the dialog (the SavvyCAN wording).</param>
/// <param name="Pattern">Qt-style filter pattern, kept for the UI's use.</param>
/// <param name="Format">Implementation, or null for a format WinCAN cannot handle yet.</param>
public sealed record LogFormatFilter(string Name, string Pattern, ILogFormat? Format);

/// <summary>
/// SavvyCAN's <c>loadFilters()</c> / <c>saveFilters()</c> tables and its autodetect order.
/// Filter indices are part of the UI contract, so they must stay in the reference order even
/// though some entries (BLF, pcap, Tesla AP, CANServer, Kvaser, CLX000) have no loader yet.
/// </summary>
public static class LogFormatRegistry
{
    public static NativeCsvFormat NativeCsv { get; } = new();
    public static CrtdFormat Crtd { get; } = new();
    public static BusMasterFormat BusMaster { get; } = new();
    public static MicrochipFormat Microchip { get; } = new();
    public static VectorTraceFormat VectorTrace { get; } = new();
    public static IxxatMiniLogFormat IxxatMiniLog { get; } = new();
    public static CanDoFormat CanDo { get; } = new();
    public static VehicleSpyFormat VehicleSpy { get; } = new();
    public static CandumpFormat Candump { get; } = new();
    public static LawicelFormat Lawicel { get; } = new();
    public static PcanFormat Pcan { get; } = new();
    public static CanalyzerAscFormat CanalyzerAsc { get; } = new();
    public static CarBusFormat CarBus { get; } = new();
    public static CanHackerFormat CanHacker { get; } = new();
    public static GenericCsvFormat GenericCsv { get; } = new();
    public static CabanaFormat Cabana { get; } = new();
    public static CanOpenFormat CanOpen { get; } = new();
    public static IxxatCsvFormat IxxatCsv { get; } = new();
    public static CanWiseFormat CanWise { get; } = new();

    /// <summary>Load dialog entries; the index is the <c>filterIndex</c> of <see cref="Load"/>.</summary>
    public static IReadOnlyList<LogFormatFilter> LoadFilters { get; } =
    [
        new("Autodetect File Type (*.*)", "*.*", null),                                      // 0
        new("GVRET Logs (*.csv *.CSV)", "*.csv;*.CSV", NativeCsv),                            // 1
        new("CRTD Logs (*.crt *.crtd *.CRT *.CRTD)", "*.crt;*.crtd;*.CRT;*.CRTD", Crtd),      // 2
        new("BusMaster Log (*.log *.LOG)", "*.log;*.LOG", BusMaster),                         // 3
        new("Microchip Log (*.can *.CAN *.log *.LOG)", "*.can;*.CAN;*.log;*.LOG", Microchip), // 4
        new("Vector trace files (*.trace *.TRACE)", "*.trace;*.TRACE", VectorTrace),          // 5
        new("IXXAT MiniLog (*.csv *.CSV)", "*.csv;*.CSV", IxxatMiniLog),                      // 6
        new("CAN-DO Log (*.avc *.can *.evc *.qcc *.AVC *.CAN *.EVC *.QCC)", "*.avc;*.can;*.evc;*.qcc;*.AVC;*.CAN;*.EVC;*.QCC", CanDo), // 7
        new("Vehicle Spy (*.csv *.CSV)", "*.csv;*.CSV", VehicleSpy),                          // 8
        new("Candump/Kayak (*.log *.LOG)", "*.log;*.LOG", Candump),                           // 9
        new("CANDump Lawicel (*.txt *.TXT *.LOG *.log)", "*.txt;*.TXT;*.LOG;*.log", Lawicel), // 10
        new("PCAN Viewer (*.trc *.TRC)", "*.trc;*.TRC", Pcan),                                // 11
        new("Kvaser Log Decimal (*.txt *.TXT)", "*.txt;*.TXT", null),                         // 12
        new("Kvaser Log Hex (*.txt *.TXT)", "*.txt;*.TXT", null),                             // 13
        new("CANalyzer Ascii Log (*.asc *.ASC)", "*.asc;*.ASC", CanalyzerAsc),                // 14
        new("CANalyzer Binary Log Files (*.blf *.BLF)", "*.blf;*.BLF", null),                 // 15
        new("CARBUS Analyzer Trace Files (*.trc *.TRC)", "*.trc;*.TRC", CarBus),              // 16
        new("CANHacker Trace Files (*.trc *.TRC)", "*.trc;*.TRC", CanHacker),                 // 17
        new("Generic ID/Data CSV (*.csv *.CSV)", "*.csv;*.CSV", GenericCsv),                  // 18
        new("Cabana Log (*.csv *.CSV)", "*.csv;*.CSV", Cabana),                               // 19
        new("CANOpen Magic (*.csv *.CSV)", "*.csv;*.CSV", CanOpen),                           // 20
        new("Tesla Autopilot Snapshot (*.CAN *.can)", "*.CAN;*.can", null),                   // 21
        new("CLX000 (*.txt *.TXT)", "*.txt;*.TXT", null),                                     // 22
        new("CANServer Binary Log (*.log *.LOG)", "*.log;*.LOG", null),                       // 23
        new("Wireshark (*.pcap *.PCAP *.pcapng *.PCAPNG)", "*.pcap;*.PCAP;*.pcapng;*.PCAPNG", null), // 24
        new("Wireshark SocketCAN (*.pcap *.PCAP)", "*.pcap;*.PCAP", null),                    // 25
        new("IXXAT CSV export (*.csv)", "*.csv", IxxatCsv),                                   // 26
        new("CANWise Log (*.log)", "*.log", CanWise),                                         // 27
    ];

    /// <summary>Save dialog entries; the index is the <c>filterIndex</c> of <see cref="Save"/>.</summary>
    public static IReadOnlyList<LogFormatFilter> SaveFilters { get; } =
    [
        new("GVRET Logs (*.csv *.CSV)", "*.csv;*.CSV", NativeCsv),                            // 0
        new("CRTD Logs (*.crt *.crtd *.CRT *.CRTD)", "*.crt;*.crtd;*.CRT;*.CRTD", Crtd),      // 1
        new("Generic ID/Data CSV (*.csv *.CSV)", "*.csv;*.CSV", GenericCsv),                  // 2
        new("BusMaster Log (*.log *.LOG)", "*.log;*.LOG", BusMaster),                         // 3
        new("Microchip Log (*.can *.CAN *.log *.LOG)", "*.can;*.CAN;*.log;*.LOG", Microchip), // 4
        new("Vector Trace Files (*.trace *.TRACE)", "*.trace;*.TRACE", VectorTrace),          // 5
        new("IXXAT MiniLog (*.csv *.CSV)", "*.csv;*.CSV", IxxatMiniLog),                      // 6
        new("CAN-DO Log (*.can *.avc *.evc *.qcc *.CAN *.AVC *.EVC *.QCC)", "*.can;*.avc;*.evc;*.qcc;*.CAN;*.AVC;*.EVC;*.QCC", CanDo), // 7
        new("Candump/Kayak (*.log)", "*.log", Candump),                                       // 8
        new("Cabana Log (*.csv *.CSV)", "*.csv;*.CSV", Cabana),                               // 9
        new("CANalyzer Ascii Log (*.asc *.ASC)", "*.asc;*.ASC", CanalyzerAsc),                // 10
        new("CARBUS Analyzer (*.trc *.TRC)", "*.trc;*.TRC", CarBus),                          // 11
        new("PCAN Viewer (*.trc *.TRC)", "*.trc;*.TRC", Pcan),                                // 12
    ];

    /// <summary>
    /// SavvyCAN's autodetect order. Formats WinCAN cannot read yet are simply absent, so a BLF
    /// or pcap still ends up reported as "could not autodetect" rather than misdetected.
    /// </summary>
    private static readonly ILogFormat[] DetectionOrder =
    [
        NativeCsv,
        CanalyzerAsc,
        Crtd,
        VectorTrace,
        VehicleSpy,
        Candump,
        CarBus,
        CanHacker,
        Cabana,
        CanOpen,
        BusMaster,
        Pcan,
        IxxatMiniLog,
        Microchip,
        CanDo,
        Lawicel,
        CanWise,
        IxxatCsv,
        GenericCsv,
    ];

    /// <summary>Loads using an explicit filter index; index 0 means autodetect.</summary>
    public static bool Load(string path, int filterIndex, List<CanFrame> frames)
    {
        if (filterIndex <= 0 || filterIndex >= LoadFilters.Count)
        {
            return AutoDetectLoad(path, frames);
        }

        var format = LoadFilters[filterIndex].Format;
        if (format is null)
        {
            return false;
        }

        var start = frames.Count;
        if (format.Load(path, frames))
        {
            return true;
        }

        frames.RemoveRange(start, frames.Count - start);
        return false;
    }

    /// <summary>Saves using a save-filter index.</summary>
    public static bool Save(string path, int filterIndex, IReadOnlyList<CanFrame> frames)
    {
        if (filterIndex < 0 || filterIndex >= SaveFilters.Count)
        {
            return false;
        }

        var format = SaveFilters[filterIndex].Format;
        return format is not null && format.CanSave && format.Save(path, frames);
    }

    /// <summary>
    /// Tries every known format in SavvyCAN's order and discards partial results from each
    /// failed attempt, so a misdetection cannot pollute the frame list.
    /// </summary>
    public static bool AutoDetectLoad(string path, List<CanFrame> frames)
    {
        foreach (var format in DetectionOrder)
        {
            bool matched;
            try
            {
                matched = format.IsMatch(path);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                continue;
            }

            if (!matched)
            {
                continue;
            }

            var start = frames.Count;
            if (format.Load(path, frames))
            {
                return true;
            }

            frames.RemoveRange(start, frames.Count - start);
        }

        return false;
    }

    /// <summary>Returns the format that would load <paramref name="path"/>, for tests and UI hints.</summary>
    public static ILogFormat? AutoDetect(string path)
    {
        var scratch = new List<CanFrame>();
        foreach (var format in DetectionOrder)
        {
            bool matched;
            try
            {
                matched = format.IsMatch(path);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                continue;
            }

            if (!matched)
            {
                continue;
            }

            scratch.Clear();
            if (format.Load(path, scratch))
            {
                return format;
            }
        }

        return null;
    }

    /// <summary>Finds a format by its display name (used by settings and tests).</summary>
    public static ILogFormat? FindByName(string name)
    {
        foreach (var filter in LoadFilters)
        {
            if (filter.Format is not null &&
                string.Equals(filter.Format.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return filter.Format;
            }
        }

        return null;
    }
}
