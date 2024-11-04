
namespace Daily.Data
{
    public class TextWriter
    {
        private const bool append = false;

        public void WriteText(string path, string value)
        {
            using (StreamWriter writer = new StreamWriter(path, append))
            {
                writer.Write(value);
            }
        }

        public async Task WriteTextAsync(string path, string value)
        {
            using (StreamWriter writer = new StreamWriter(path, append))
            {
                await writer.WriteAsync(value);
            }
        }

        public string ReadText(string path)
        {
            using (StreamReader writer = new StreamReader(path))
            {
                return writer.ReadLine()!;
            }
        }
    }
}