# Sereyn CMS

Is intended to be a simple CMS type solution. The system makes use of what I refer to as content files which are a mixed JSON and Markdown document. The initial idea for this project was born from looking at static site generators such as Jekyll. I wanted to replicate the simplicity of managing content using a combination of front matter and markdown you see in Jekyll but using Blazor.

IMPORTANT - Until I reach a version 1.0.0 build, you can expect new versions may introduce breaking changes. That said, I welcome feedback and feature requests. 

## How does it work?

As of version 0.1.0, you host on your web server or web-accessible blob storage two folders which are the Content and Catalogues folders:

### Catalogues

In the Catalogues folder, you have JSON documents that contain metadata for your content along with the web location of each of the content files. You will use the contents of these files to build navigation menus and content listing within your application. They contain the exact web address where the individual content file exists without the need to query a back-end web service. These exist to support Blazor WASM/PWA scenarios. 

### Content

In the Content folder, you will have a modified markdown file. At the top of the content file, you will have a JSON object that contains the configuration data for your page. The configuration data is exposed as an IConfiguration object that is attached to the content model. The content model has only two properties at this time, the aforementioned IConfiguration property and a string property that contains the HTML from the markdown within the content file. At this time the solution uses MarkDig to parse Markdown to HTML. Sereyn CMS uses the Advanced Extensions pipeline in MarkDig to allow more customisation options for how your markdown will be rendered into HTML. 

## Installation

### Step 1 – Install the package via NuGet 
```
Install-Package Sereyn.CMS
``` 
### Step 2 – Add the Sereyn CMS services.  
WASM – Add the following line to the Main method in Program.cs  
```
builder.Services.AddSereynCMS();
```  
Server – Add the following line to the ConfigureServices method in Startup.cs  
```
Services.AddSereynCMS();
```  
If Visual Studio doesn’t add it automatically, you will need to add the following using statement.  
```
using Sereyn.CMS;
```

### Step 3 – Create the appsettings.json file in wwwroot, and add the following SereynCMS node. 
```
{
    "SereynCMS": {
        "BaseUrl": "https://<URL for the site hosting your content and catalogues>",
        "Catalogues": {
            "Folder": "Catalogues",
            "Category": "CategoryCatalogue.json",
            "Content": "ContentCatalogue.json"
        },
        "Content": {
            "Folder": "Content"
        }
    }
}
```

### Step 4 - Take a look at an example project I threw together. 

I have put together an example Blazor WASM project which you can find on the Sereyn.CMS.Examples repository. You can see this example running at the following URL.

Repo Link: https://github.com/TheSereyn/Sereyn.CMS.Examples  
Live Example: https://sereynblogexample.z16.web.core.windows.net

## Running the Catalogue Builder

The only element of the Sereyn CMS solution that needs to be generated are the Catalogues. These are generated using the Catalogue Builder tool. To generate the catalogues do the following:

Download the approriate version of the Catalogue Builder from the release page. Then run the following command: 

**Linux**
```
Sereyn.CMS.CatalogueBuilder-linux-x64 "<Content Folder location>" "<Catalogue Folder Location>"
```

**Windows**
```
Sereyn.CMS.CatalogueBuilder-win-x64.exe "<Content Folder location>" "<Catalogue Folder Location>"
```

I do plan to wrap this functionality into a GitHub Action in the future. Though for the moment, you have to run it everytime you want to generate the catalogues. 