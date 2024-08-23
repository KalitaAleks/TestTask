using Microsoft.AspNetCore.Mvc;
namespace TestTask.Objects
{
    public class HtmlResult : IActionResult
    {
        //string htmlCode;
       // public HtmlResult();
        public async Task ExecuteResultAsync(ActionContext context)
        {
            string fullHtmlCode = @$"<!DOCTYPE html>
            <html>
            <head>
               <meta charset=""utf-8"">
                <title>TestTask</title>
                <style>
                    td {{
                        padding: 5px;
                    }}
                    button {{
                        margin: 5px;
                    }}
                </style>
            </head>
            <body>
                <h2>Запросы</h2>
                <div>
                    <p>
                        <button id=""GetAllData"">Весь отчет</button>
                    </p>
                    <p>
                        <button id=""getScan"">Данные о сканировании</button>
                    </p>
                    <p>
                        Получение списка имен файлов<br />
                        <input type=""checkbox"" id=""value"" title=""Только файлы с ошибками"" value=""true"" />
                        <button id=""getAllFilenames"">Список имен файлов</button>
                    </p>
                    <p>
                        Получение данных о файлах с ошибками, <br /> в поле вводится порядковый номер файла начиная с 0,<br /> пустое поле все файлы<br />
                        <input id=""index"" , type=""number"" />
                        <button id=""GetErrorsFiles"">Получить</button>
                    </p>
                    <p>
                        <button id=""GetErorsCount"">Количество ошибок</button>
                    </p>
                    <p>
                        <button id=""GetQuerys"">Выборки</button>
                    </p>

                    <p>
                        <button id=""GetServiceInfo"">Информмация о сервере</button>
                    </p>
                    <p>
                        Ввод новых данных строка в формате Json:<br />
                        <input id=""strJson"" />
                    </p>

                    <p>
                        <button id=""saveBtn"">Сохранить</button>
                        <button id=""resetBtn"">Сбросить</button>
                    </p>
                    <text id=""data"" />
                </div>
                <script>
                // Получение всех данных
                    async function getAllData() {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/allData"", {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}
                        else {{
                            const error = await response.json();
                            console.log(error.message);
                            const Field = document.getElementById(""data"");
                            Field.innerText = error.message;
                        }}
                    }}
                    async function getScan() {{
                            // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/Scan"", {{
                                method: ""GET"",
                                headers: {{ ""Accept"": ""application/json"" }}
                            }});
                            // если запрос прошел нормально
                            if (response.ok === true) {{
                                // получаем данные
                                const Data = await response.text();
                                const Field = document.getElementById(""data"");
                                Field.innerText = Data;
                        }}
                            else {{
                                const error = await response.json();
                                console.log(error.message);
                                const Field = document.getElementById(""data"");
                                Field.innerText = error.message;
                            }}
                    }}

                    async function GetErrorsFilesByIndex(index) {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(`/api/errors/${{index}}`, {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}
                        else {{
                            const error = await response.json();
                            console.log(error.message);
                            const Field = document.getElementById(""data"");
                            Field.innerText = error.message;
                        }}
                    }}
                    async function GetErrorsFiles() {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/errors"", {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}

                    }}

                    async function GetQuerys() {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/query/check"", {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}

                    }}

                    async function GetErorsCount() {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/errors/count"", {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}

                    }}

                    async function getAllFilenames(value) {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(`/api/filenames?correct=${{value}}`, {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}

                    }}
        
                    async function GetServiceInfo() {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/service/serviceInfo"", {{
                            method: ""GET"",
                            headers: {{ ""Accept"": ""application/json"" }}
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText = Data;
                        }}

                    }}

                    async function CreateNewData(Json) {{
                        // отправляет запрос и получаем ответ
                        const response = await fetch(""/api/newErrors"", {{
                            method: ""POST"",
                            headers: {{ ""Accept"": ""application/json"", ""Content-Type"": ""application/json"" }},
                            body: JSON.stringify(Json)
                        }});
                        // если запрос прошел нормально
                        if (response.ok === true) {{
                            // получаем данные
                            const Data = await response.text();
                            const Field = document.getElementById(""data"");
                            Field.innerText =  Data;
                        }}
                        else {{

                            const error = await response.json();
                            console.log(error.message);
                            const Field = document.getElementById(""data"");
                            Field.innerText = error.message;
                        }}
                    }}
        
                    // сброс данных формы после отправки
                    function reset() {{
                        document.getElementById(""strJson"").value = """";
                    }}

                    // сброс значений формы
                    document.getElementById(""resetBtn"").addEventListener(""click"", () =>  reset());

                    // отправка формы
                    document.getElementById(""saveBtn"").addEventListener(""click"", async () => {{

                        const Json = document.getElementById(""strJson"").value;
                        await CreateNewData(Json);

                        reset();
                    }});

                    document.getElementById(""GetAllData"").addEventListener(""click"", async () => getAllData());
                    document.getElementById(""getScan"").addEventListener(""click"", async () => getScan());
                    document.getElementById(""GetErrorsFiles"").addEventListener(""click"", async () => {{
                        const val = document.getElementById(""index"").value;
                        if (val === """")
                            GetErrorsFiles();
                        else
                            GetErrorsFilesByIndex(val);
                    }});

                    document.getElementById(""GetQuerys"").addEventListener(""click"", async () => GetQuerys());
                    document.getElementById(""GetErorsCount"").addEventListener(""click"", async () => GetErorsCount());
                    document.getElementById(""getAllFilenames"").addEventListener(""click"", async () => {{
                        const boolValue = document.getElementById(""value"");
                        if (boolValue.checked===false )
                        {{
                
                            getAllFilenames(true);
                        }}
                        else 
                        {{
                            getAllFilenames(false);
                        }}
                    }});
                    document.getElementById(""GetServiceInfo"").addEventListener(""click"", async () => GetServiceInfo());
                </script>
            </body>
            </html>
			";
            await context.HttpContext.Response.WriteAsync(fullHtmlCode);
        }
    }
}
