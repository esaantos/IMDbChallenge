# IMDbChallenge

# IMDb Crawler e RPA

Este projeto consiste em duas partes principais:
1. **Crawler**: Extrai informações dos 20 primeiros filmes da lista dos 250 melhores do IMDb.
2. **RPA**: Automatiza o login no IMDb e navega até a página de classificações da conta.

---

## Funcionalidades

### 1. Crawler
- Acessa a lista dos 250 melhores filmes do IMDb.
- Extrai as seguintes informações dos 20 primeiros filmes:
  - Nome do filme
  - Ano de lançamento
  - Nome do diretor
  - Avaliação média (0 a 10 estrelas)
  - Número de avaliações
- Salva os dados em um arquivo CSV (`movies.csv`).

### 2. RPA
- Automatiza o login no IMDb usando credenciais configuradas.
- Navega até a página de classificações da conta.
- Simula comportamento humano para evitar detecção de automação.

---

## Requisitos

### Software
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Google Chrome](https://www.google.com/chrome/) (ou outro navegador suportado pelo Selenium)
- [ChromeDriver](https://sites.google.com/chromium.org/driver/) (compatível com a versão do Chrome instalada)

### Pacotes NuGet
- `HtmlAgilityPack`
- `CsvHelper`
- `Selenium.WebDriver`
- `Selenium.WebDriver.ChromeDriver`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Configuration.Json`
- `Microsoft.Extensions.Logging.Console`

---

## Execução

### 1. Instalação das Dependências
No terminal, navegue até a pasta do projeto e execute:
dotnet restore

### 2. Executar o Crawler
Para executar o Crawler e extrair os dados dos filmes:
dotnet run --project IMDbCrawler.Console
* O arquivo movies.csv será gerado na pasta Downloads.

### 3. Executar o RPA
Para executar o RPA e automatizar o login no IMDb:
dotnet run --project IMDbRPA.Console
* O navegador será aberto, e o script fará login e navegará até a página de classificações.
