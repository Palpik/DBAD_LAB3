using AdAgency.Domain;
using Microsoft.EntityFrameworkCore;
using AdAgency.WebApp.Services;


var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AdAgencyContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<ICachedAdTypesServices, CachedAdTypesServices>();
builder.Services.AddMemoryCache();


// добавление поддержки сессии
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var app = builder.Build();

app.UseSession();

app.Map("/table", async (context) =>
{
        ICachedAdTypesServices cachedAdTypesServices = context.RequestServices.GetService<ICachedAdTypesServices>();
        IEnumerable<AdType> adTypes = cachedAdTypesServices.GetAdTypes("adTypes20");
        string HtmlString = "<HTML><HEAD><TITLE>Рекламное агенство</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список типов рекламы</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Тип рекламы</TH>";
        HtmlString += "<TH>Описание</TH>";
        HtmlString += "</TR>";
        
        foreach (var adType in adTypes)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + adType.Id + "</TD>";
            HtmlString += "<TD>" + adType.Name + "</TD>";
            HtmlString += "<TD>" + adType.Description + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "<BR><A href='/table'>Таблица</A></BR>";
        HtmlString += "<BR><A href='/searchform1'>Форма поиска 1</A></BR>";
        HtmlString += "<BR><A href='/searchform2'>Форма поиска 2</A></BR>";
        HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
        HtmlString += "</BODY></HTML>";

        // Вывод
        await context.Response.WriteAsync(HtmlString);
});

app.Map("/info", async context =>
{
    
    string HtmlString = "<HTML><HEAD><TITLE>Рекламное агенство</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>Информация</H1>";
    HtmlString += "<BR> Сервер: " + context.Request.Host;
    HtmlString += "<BR> Путь: " + context.Request.PathBase;
    HtmlString += "<BR> Протокол: " + context.Request.Protocol;
    HtmlString += "<BR><A href='/'>Главная</A></BODY></HTML>";
    HtmlString += "<BR><A href='/table'>Таблица</A></BR>";
    HtmlString += "<BR><A href='/searchform1'>Форма поиска 1</A></BR>";
    HtmlString += "<BR><A href='/searchform2'>Форма поиска 2</A></BR>";
    HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
    HtmlString += "</BODY></HTML>";
    await context.Response.WriteAsync(HtmlString);
});

app.Map("/searchform1", async context =>
{
    ICachedAdTypesServices cachedAdTypesServices = context.RequestServices.GetService<ICachedAdTypesServices>();
    IEnumerable<AdType> adTypes = cachedAdTypesServices.GetAdTypes("Tanks20");

    string inputKey = "inputKey";
    string selectKey = "selectKey";
    string mSelectKey = "mSelectKey";

    string HtmlString = "<HTML><HEAD><TITLE>Рекламное агенство</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>Форма поиска 1 (Куки)</H1>";
    
    HtmlString += "<form>" +
    $"<input type=\"text\" value=\"{context.Request.Cookies[inputKey]}\" name=\"{inputKey}\">" +
    $"<select name=\"{selectKey}\"><option>Default</option>";
    foreach (var adType in adTypes)
    {
        if ($"{adType.Id}. {adType.Name}" == context.Request.Cookies[selectKey])
            HtmlString += $"<option selected>{adType.Id}. {adType.Name}</option>";
        else
            HtmlString+= $"<option>{adType.Id}. {adType.Name}</option>";
    }

    HtmlString += $"</select><select multiple name=\"{mSelectKey}\">";

    string[] selectedItemsArr = { };
    if (context.Request.Cookies[mSelectKey] is not null)
        selectedItemsArr = context.Request.Cookies[mSelectKey].Split(',');

    foreach (var adType in adTypes)
    {
        bool isContains = false;
        foreach (var item in selectedItemsArr)
        {
            if ($"{adType.Id}. {adType.Name}" == item)
            {
                isContains = true;
                break;
            }
        }

        if (isContains)
            HtmlString += $"<option selected>{adType.Id}. {adType.Name}</option>";
        else
            HtmlString += $"<option>{adType.Id}. {adType.Name}</option>";
    }

    HtmlString += "</select><br>" +
        "<button type=\"submit\">Кнопка</button>" +
        "</form>";
    string inputStr = context.Request.Query[inputKey];
    string selectStr = context.Request.Query[selectKey];
    string mSelectStr = context.Request.Query[mSelectKey];
    
    if (inputStr is not null)
        context.Response.Cookies.Append(inputKey, inputStr);

    if (selectStr is not null)
        context.Response.Cookies.Append(selectKey, selectStr);

    if (mSelectStr is not null)
        context.Response.Cookies.Append(mSelectKey, mSelectStr);

    await context.Response.WriteAsync(HtmlString);
});


app.Map("/searchform2", async context =>
{
    ICachedAdTypesServices cachedAdTypesServices = context.RequestServices.GetService<ICachedAdTypesServices>();
    IEnumerable<AdType> adTypes = cachedAdTypesServices.GetAdTypes("Tanks20");

    string inputKey = "inputKey";
    string selectKey = "selectKey";
    string mSelectKey = "mSelectKey";

    string HtmlString = "<HTML><HEAD><TITLE>Рекламное агенство</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>Форма поиска 2 (Сессия)</H1>";
    
    HtmlString += "<form>" +
    $"<input type=\"text\" value=\"{context.Session.GetString(inputKey)}\" name=\"{inputKey}\">" +
    $"<select name=\"{selectKey}\"><option>Default</option>";
    foreach (var adType in adTypes)
    {
        if ($"{adType.Id}. {adType.Name}" == context.Session.GetString(selectKey))
            HtmlString += $"<option selected>{adType.Id}. {adType.Name}</option>";
        else
            HtmlString+= $"<option>{adType.Id}. {adType.Name}</option>";
    }

    HtmlString += $"</select><select multiple name=\"{mSelectKey}\">";

    string[] selectedItemsArr = { };
    if (context.Session.GetString(mSelectKey) is not null)
        selectedItemsArr = context.Session.GetString(mSelectKey).Split(',');

    foreach (var adType in adTypes)
    {
        bool isContains = false;
        foreach (var item in selectedItemsArr)
        {
            if ($"{adType.Id}. {adType.Name}" == item)
            {
                isContains = true;
                break;
            }
        }

        if (isContains)
            HtmlString += $"<option selected>{adType.Id}. {adType.Name}</option>";
        else
            HtmlString += $"<option>{adType.Id}. {adType.Name}</option>";
    }

    HtmlString += "</select><br>" +
        "<button type=\"submit\">Кнопка</button>" +
        "</form>";
    string inputStr = context.Request.Query[inputKey];
    string selectStr = context.Request.Query[selectKey];
    string mSelectStr = context.Request.Query[mSelectKey];
    
    if (inputStr is not null)
        context.Session.SetString(inputKey, inputStr);

    if (selectStr is not null)
        context.Session.SetString(selectKey, selectStr);

    if (mSelectStr is not null)
        context.Session.SetString(mSelectKey, mSelectStr);

    await context.Response.WriteAsync(HtmlString);
});


app.Map("/", async context =>
{
    ICachedAdTypesServices cachedAdTypesServices = context.RequestServices.GetService<ICachedAdTypesServices>();
    cachedAdTypesServices.AddAdTypes("Tanks20");
    string HtmlString = "<HTML><HEAD><TITLE>Рекламное агенство</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>Главная</H1>";
    HtmlString += "<BR><A href='/'>Главная</A></BR>";
    HtmlString += "<BR><A href='/table'>Таблица</A></BR>";
    HtmlString += "<BR><A href='/searchform1'>Форма поиска 1</A></BR>";
    HtmlString += "<BR><A href='/searchform2'>Форма поиска 2</A></BR>";
    HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
    HtmlString += "</BODY></HTML>";

    await context.Response.WriteAsync(HtmlString);
});

app.Run();