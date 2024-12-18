using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class CharacterData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("imageurl")]
    public string ImageUrl { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public class CharacterResponse
{
    [JsonProperty("data")]
    public List<CharacterData> Data { get; set; }

    [JsonProperty("meta")]
    public PaginationMeta Meta { get; set; }
}

public class PaginationMeta
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("pageCount")]
    public int PageCount { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
}