using System.Text.Json;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Options;
using Quiz.Master.Core.Models;
using Quiz.Master.Features.Game.CreateGame;

namespace Quiz.Master.Services.ContentManagement; 

public class ContentManagementClient : IContentManagementClient
{
    private readonly GraphQLHttpClient client;

    public ContentManagementClient(IOptions<ContentManagementConfig> options)
    {
        var uri = new Uri(options.Value.BaseUrl);
        var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new MiniGameDtoConverter() }
        });
        var httpClient = new HttpClient
        {
            DefaultRequestHeaders = { { "Authorization", $"Bearer {options.Value.Token}" } }
        };

        client = new GraphQLHttpClient(uri, serializer, httpClient);
    }

    public async Task<SimpleGameDefinition> GetGameDefinition(string indentifier, GameLanguage language) {
        var definition = new GraphQLRequest
        {
            Query = """
            query QueryGameDefinitions($locale: String!, $identifier: String!) {
                gameDefinitions(filters: { 
                    identifier: { eq: $identifier }, 
                    locale: { eq: $locale } 
                }) {
                    identifier,
                    name,
                    locale,
                    createdAt,
                    miniGames {
                    __typename,
                    ... on ComponentSharedMiniGameDefAbcd {
                        rounds {
                            id,
                            categories {
                                id,
                                name,
                                questions {
                                    id,
                                    text,
                                    answers {
                                        id,
                                        isCorrect,
                                        text
                                    }
                                    audio {
                                        name,
                                        url,
                                        ext
                                    }
                                }
                            }
                        }
                    }
                    ... on ComponentSharedMinDefMusic {
                        id,
                        rounds {
                            id,
                            categories {
                                id,
                                name,
                                musicQuestions: questions {
                                    id,
                                    text,
                                    answers {
                                        id,
                                        isCorrect,
                                        text
                                    },
                                    audio {
                                        name,
                                        url,
                                        ext
                                    }   
                                }
                            }
                        }
                    }
                    }
                } 
            }
            """,
            Variables = new
            {
                locale = language switch
                {
                    GameLanguage.English => "en",
                    GameLanguage.Polish => "pl",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                identifier = indentifier

            }
        };

        var response = await client.SendQueryAsync<Response>(definition);
        var def = response.Data.GameDefinitions.FirstOrDefault();
       
        if(def == null) {
            throw new InvalidOperationException("Game definition not found");
        }

        return new SimpleGameDefinition(def.Identifier, def.Name, def.Locale, def.MiniGames.Select(mg => new MiniGameDefinition {
             Type = mg.GameType,
             CreatedAt = DateTime.UtcNow,
             Definition = mg.MapToDefinition(),
        }));
    }

    public record SimpleGameDefinition(string Identifier, string Name, string Locale, IEnumerable<MiniGameDefinition> MiniGames);

    public record Response {
        public List<GameDefinition> GameDefinitions { get; set; } = new();

        public record GameDefinition
        {
            public required string Identifier { get; set; }
            public required string Name { get; set; }
            public required string Locale { get; set; }
            public List<MiniGameDto> MiniGames { get; set; } = new();
        }
    }
}
