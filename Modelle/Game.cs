using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BlazorApp
{
    public class Game
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("name_original", NullValueHandling = NullValueHandling.Ignore)]
        public string NameOriginal { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("metacritic", NullValueHandling = NullValueHandling.Ignore)]
        public long? Metacritic { get; set; }

        [JsonProperty("metacritic_platforms", NullValueHandling = NullValueHandling.Ignore)]
        public List<MetacriticPlatform> MetacriticPlatforms { get; set; }

        [JsonProperty("released", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Released { get; set; }

        [JsonProperty("tba", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tba { get; set; }

        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Updated { get; set; }

        [JsonProperty("background_image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri BackgroundImage { get; set; }

        [JsonProperty("background_image_additional", NullValueHandling = NullValueHandling.Ignore)]
        public Uri BackgroundImageAdditional { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Website { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double? Rating { get; set; }

        [JsonProperty("rating_top", NullValueHandling = NullValueHandling.Ignore)]
        public long? RatingTop { get; set; }

        [JsonProperty("ratings", NullValueHandling = NullValueHandling.Ignore)]
        public List<Rating> Ratings { get; set; }

        [JsonProperty("reactions", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> Reactions { get; set; }

        [JsonProperty("added", NullValueHandling = NullValueHandling.Ignore)]
        public long? Added { get; set; }

        [JsonProperty("added_by_status", NullValueHandling = NullValueHandling.Ignore)]
        public AddedByStatus AddedByStatus { get; set; }

        [JsonProperty("playtime", NullValueHandling = NullValueHandling.Ignore)]
        public long? Playtime { get; set; }

        [JsonProperty("screenshots_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ScreenshotsCount { get; set; }

        [JsonProperty("movies_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? MoviesCount { get; set; }

        [JsonProperty("creators_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreatorsCount { get; set; }

        [JsonProperty("achievements_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? AchievementsCount { get; set; }

        [JsonProperty("parent_achievements_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentAchievementsCount { get; set; }

        [JsonProperty("reddit_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedditUrl { get; set; }

        [JsonProperty("reddit_name", NullValueHandling = NullValueHandling.Ignore)]
        public string RedditName { get; set; }

        [JsonProperty("reddit_description", NullValueHandling = NullValueHandling.Ignore)]
        public string RedditDescription { get; set; }

        [JsonProperty("reddit_logo", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedditLogo { get; set; }

        [JsonProperty("reddit_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? RedditCount { get; set; }

        [JsonProperty("twitch_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? TwitchCount { get; set; }

        [JsonProperty("youtube_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? YoutubeCount { get; set; }

        [JsonProperty("reviews_text_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReviewsTextCount { get; set; }

        [JsonProperty("ratings_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? RatingsCount { get; set; }

        [JsonProperty("suggestions_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? SuggestionsCount { get; set; }

        [JsonProperty("alternative_names", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AlternativeNames { get; set; }

        [JsonProperty("metacritic_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri MetacriticUrl { get; set; }

        [JsonProperty("parents_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentsCount { get; set; }

        [JsonProperty("additions_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? AdditionsCount { get; set; }

        [JsonProperty("game_series_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameSeriesCount { get; set; }

        [JsonProperty("user_game")]
        public object UserGame { get; set; }

        [JsonProperty("reviews_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReviewsCount { get; set; }

        [JsonProperty("saturated_color", NullValueHandling = NullValueHandling.Ignore)]
        public string SaturatedColor { get; set; }

        [JsonProperty("dominant_color", NullValueHandling = NullValueHandling.Ignore)]
        public string DominantColor { get; set; }

        [JsonProperty("parent_platforms", NullValueHandling = NullValueHandling.Ignore)]
        public List<ParentPlatform> ParentPlatforms { get; set; }

        [JsonProperty("platforms", NullValueHandling = NullValueHandling.Ignore)]
        public List<PlatformElement> Platforms { get; set; }

        [JsonProperty("stores", NullValueHandling = NullValueHandling.Ignore)]
        public List<Store> Stores { get; set; }

        [JsonProperty("developers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Developer> Developers { get; set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public List<Developer> Genres { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<Developer> Tags { get; set; }

        [JsonProperty("publishers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Developer> Publishers { get; set; }

        [JsonProperty("esrb_rating", NullValueHandling = NullValueHandling.Ignore)]
        public EsrbRating EsrbRating { get; set; }

        [JsonProperty("clip")]
        public object Clip { get; set; }

        [JsonProperty("description_raw", NullValueHandling = NullValueHandling.Ignore)]
        public string DescriptionRaw { get; set; }
    }

    public partial class AddedByStatus
    {
        [JsonProperty("yet", NullValueHandling = NullValueHandling.Ignore)]
        public long? Yet { get; set; }

        [JsonProperty("owned", NullValueHandling = NullValueHandling.Ignore)]
        public long? Owned { get; set; }

        [JsonProperty("beaten", NullValueHandling = NullValueHandling.Ignore)]
        public long? Beaten { get; set; }

        [JsonProperty("toplay", NullValueHandling = NullValueHandling.Ignore)]
        public long? Toplay { get; set; }

        [JsonProperty("dropped", NullValueHandling = NullValueHandling.Ignore)]
        public long? Dropped { get; set; }

        [JsonProperty("playing", NullValueHandling = NullValueHandling.Ignore)]
        public long? Playing { get; set; }
    }

    public partial class Developer
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("games_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GamesCount { get; set; }

        [JsonProperty("image_background", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageBackground { get; set; }

        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string Domain { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public Language? Language { get; set; }
    }

    public partial class EsrbRating
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }
    }

    public partial class MetacriticPlatform
    {
        [JsonProperty("metascore", NullValueHandling = NullValueHandling.Ignore)]
        public long? Metascore { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public MetacriticPlatformPlatform Platform { get; set; }
    }

    public partial class MetacriticPlatformPlatform
    {
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public long? Platform { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }
    }

    public partial class ParentPlatform
    {
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public EsrbRating Platform { get; set; }
    }

    public partial class PlatformElement
    {
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public PlatformPlatform Platform { get; set; }

        [JsonProperty("released_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ReleasedAt { get; set; }

        [JsonProperty("requirements", NullValueHandling = NullValueHandling.Ignore)]
        public Requirements Requirements { get; set; }
    }

    public partial class PlatformPlatform
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("image")]
        public object Image { get; set; }

        [JsonProperty("year_end")]
        public object YearEnd { get; set; }

        [JsonProperty("year_start")]
        public object YearStart { get; set; }

        [JsonProperty("games_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GamesCount { get; set; }

        [JsonProperty("image_background", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageBackground { get; set; }
    }

    public partial class Requirements
    {
        [JsonProperty("minimum", NullValueHandling = NullValueHandling.Ignore)]
        public string Minimum { get; set; }

        [JsonProperty("recommended", NullValueHandling = NullValueHandling.Ignore)]
        public string Recommended { get; set; }
    }

    public partial class Rating
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("percent", NullValueHandling = NullValueHandling.Ignore)]
        public double? Percent { get; set; }
    }

    public partial class Store
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("store", NullValueHandling = NullValueHandling.Ignore)]
        public Developer StoreStore { get; set; }
    }

    public enum Language
    { Eng };
}