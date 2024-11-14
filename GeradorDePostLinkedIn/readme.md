Este projeto é um gerador de posts para o LinkedIn desenvolvido em C# que automatiza a criação de conteúdo atrativo a partir de notícias extraídas de uma URL. Ele realiza as seguintes etapas:

Extração de Texto: A partir de uma URL fornecida, o projeto utiliza uma requisição HTTP para acessar e extrair o conteúdo da página.
Limpeza do Texto: O texto extraído é limpo, removendo quebras de linha e tags HTML, tornando-o mais adequado para processamento.
Integração com Azure OpenAI: O texto limpo é enviado para a API do Azure OpenAI, onde é processado com um prompt específico para gerar um post atrativo para o LinkedIn. O modelo seleciona as notícias mais interessantes e as estrutura em um formato adequado para o LinkedIn.
Exibição do Resultado: O post gerado é exibido no console.
O projeto utiliza a API do Azure OpenAI para gerar posts baseados no conteúdo extraído, ajudando a criar publicações mais envolventes e direcionadas para um público no LinkedIn.
