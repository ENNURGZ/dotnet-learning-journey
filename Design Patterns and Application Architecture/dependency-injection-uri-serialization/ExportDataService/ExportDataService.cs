using Conversion;
using DataReceiving;
using Serialization;

namespace ExportDataService;

/// <summary>
/// Presents the export data of string to type T service.
/// </summary>
/// <typeparam name="T">The type data for export.</typeparam>
public class ExportDataService<T>
{
    private readonly IDataReceiver receiver;
    private readonly IDataSerializer<T> serializer;
    private readonly IConverter<T> converter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportDataService{T}"/> class.
    /// </summary>
    /// <param name="receiver">The data receiver.</param>
    /// <param name="serializer">The data serializer.</param>
    /// <param name="converter">The data convertor.</param>
    /// <exception cref="ArgumentNullException">Trow if receiver, writer or converter is null.</exception>
    public ExportDataService(IDataReceiver receiver, IDataSerializer<T> serializer, IConverter<T> converter)
    {
        this.receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
    }

    /// <summary>
    /// Performs the process of receiving the sequence of string <see cref="IEnumerable{string}"/>
    /// obtained from <see cref="IDataReceiver"/>, then transforming it into the sequence <see cref="IEnumerable{T}"/>,
    /// and then serializing it with <see cref="IDataSerializer{T}"/>.
    /// </summary>
    public void Run()
    {
        var source = this.receiver.Receive();
        var list = new List<T>();
        foreach (var item in source)
        {
            var converted = this.converter.Convert(item);
            if (converted is not null)
            {
                list.Add(converted);
            }
        }

        this.serializer.Serialize(list);
    }
}
