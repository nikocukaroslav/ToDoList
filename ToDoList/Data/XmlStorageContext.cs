namespace ToDoList.Data;

public class XmlStorageContext
{
    private readonly IConfiguration configuration;
    private readonly string? xmlStoragePath;

    public XmlStorageContext(IConfiguration configuration)
    {
        this.configuration = configuration;
        xmlStoragePath = this.configuration.GetConnectionString("XmlStoragePath");
    }

    public string XmlStoragePath => xmlStoragePath;
}