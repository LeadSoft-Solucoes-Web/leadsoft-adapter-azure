# LeadSoft® Adapter Azure EntraID

Este pacote `Open Source` serve como uma interface simples para adicionar como injeção de dependência, a integração com o Azure EntraID, de forma mais enxuta. Ele é parte da nossa iniciativa de compartilhar conhecimento e recursos com a comunidade de desenvolvimento, permitindo que outros desenvolvedores possam se beneficiar do nosso trabalho e contribuir para o crescimento da comunidade.

Este pacote é mantido pela [LeadSoft®](https://leadsoft.com.br/), uma empresa de tecnologia que oferece soluções inovadoras para o mercado. Se você tiver alguma dúvida ou sugestão, não hesite em entrar em contato conosco.

#### [Nuget.Org: LeadSoft.Adapter.Azure.EntraID](https://www.nuget.org/packages/LeadSoft.Adapter.Azure.EntraID)
#### [GitHub Repo: leadsoft-adapter-azure](https://github.com/LeadSoft-Solucoes-Web/leadsoft-adapter-azure)

## Principais características
- Compatível com .NET 10.0.
- Chamadas assíncronas com `async`/`await`.
- Integração simples com _Dependency Injection_ (DI) do .NET.
- Tratamento centralizado de erros e respostas HTTP.
- Open Source (MIT License).
- Caso haja AWS Secrets Manager configurado, o pacote irá buscar as configurações de ambiente por lá. Caso contrário, ele buscará as variáveis de ambiente do sistema.

## Configuração e uso

1. **Instalação**: Adicione o pacote `LeadSoft.Adapter.Azure.EntraID` ao seu projeto via NuGet.
2. **Configuração**: Configure as credenciais e parâmetros necessários para a integração com o Azure EntraID (Client ID, Tenant ID, etc.) usando AWS Secrets Manager ou variáveis de ambiente.

### Aws Secrets Manager ou Variáveis de ambiente

| **Secrets Names**                       | **Descrição**                                      | 
|-----------------------------------------|----------------------------------------------------|
| AZURE_AD_CLIENT_ID                      | Azure Entra Id (AD) Client Id                      |
| AZURE_AD_TENANT_ID                      | Azure Entra Id (AD) Tenant Id                      |
| AZURE_AD_AUTH_TOKEN_CLIENT_SECRET_ID    | Azure Entra Id (AD) Auth Token Client Secret Id    |
| AZURE_AD_AUTH_TOKEN_CLIENT_SECRET_VALUE | Azure Entra Id (AD) Auth Token Client Secret Value |
| AZURE_AD_AUTH_TOKEN_REDIRECT_URL        | Azure Entra Id (AD) Auth Token Redirect URL        |
| AZURE_AD_GROUP_MEMBERS_CLIENT_ID        | Azure Entra Id (AD) Group Members Client Id        |
| AZURE_AD_GROUP_MEMBERS_CLIENT_SECRET    | Azure Entra Id (AD) Group Members Client Secret    |
| AZURE_AD_GROUP_MEMBERS_GROUP_ID         | Azure Entra Id (AD) Group Members Group Id         |
| AZURE_AD_GROUP_MEMBERS_TENANT_ID        | Azure Entra Id (AD) Group Members Tenant Id        |


## Métodos disponíveis
- `Task<bool> AddGroupMembersAsync(params string[] memberEmails)`
  -  Adiciona endereços de e-mail a um grupo de usuários específico da aplicação (App Registration).
- `Task<DTOAzureEntraIDSSOResponse> GetOAuthSSOAsync(string oAuthUserCode, bool relogin = false, bool getPicture = false)`
  -  Autentica o usuário via Azure Entra ID SSO do App Registration cadastrado.
- `Task<DTOAzureEntraIDUserProfileResponse> GetUserProfileAsync(string userEmail)`
  - Obtém o nome e avatar do perfil do usuário Entra ID pelo seu e-mail.
- `Task<string> GetEnvironmentAsync()`
  - Retorna o ambiente atual da aplicação (Desenvolvimento, Homologação ou Produção) conforme configuração de inicialização (Aws Secrets Manager ou Variáveis de ambiente).

---

## Instalação
Pelo CLI do .NET:

```bash
dotnet add package LeadSoft.Adapter.Azure.EntraID
```

Ou via NuGet Package Manager no Visual Studio (pesquise por `LeadSoft.Adapter.Azure.EntraID`).

## Versionamento e compatibilidade
- Projeto direcionado para .NET 10.0. Verifique a compatibilidade do pacote com sua aplicação.
- Seguir práticas de versionamento semântico: breaking changes → major, novas features → minor, correções → patch.

## Licença
Consulte o arquivo de licença no repositório para detalhes sobre uso e redistribuição.

## Contribuição

Se você deseja contribuir para este projeto, sinta-se à vontade para enviar pull requests ou abrir issues. Estamos sempre abertos a sugestões e melhorias.

---

###  Desenvolvimento  
Desenvolvido pelo time da LeadSoft® Soluções Web.
* [Lucas Resende Tavares](https://www.linkedin.com/in/lucasrtavares/)
  
#### Nossa empresa
**LeadSoft®** é uma marca registrada pertencente à **Lucas R Tavares Tech Ltda** | CNPJ: 31.706.323/0001-29

#### Como nos encontrar:
- [Nosso Site](https://www.leadsoft.inf.br)
- [GitHub](https://github.com/LeadSoft-Solucoes-Web)
- [LinkedIn](https://www.linkedin.com/company/leadsoft-solucoes-web)
- [Behance](https://www.behance.net/leadsofsolue)
- [YouTube](https://www.youtube.com/@LeadsoftSolucoesWeb)
- [Instagram](https://www.instagram.com/leadsoft.inf/)
- [Facebook](https://www.facebook.com/leadsoft.inf.br)

#### INFORMAÇÕES DE CONTATO  Se você tiver alguma dúvida sobre estes Termos ou Serviços, entre em contato conosco em
[developers@leadsoft.inf.br](mailto:developers@leadsoft.inf.br).
[developers@leadsoft.inf.br](mailto:developers@leadsoft.inf.br).