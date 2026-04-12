using System.ComponentModel.DataAnnotations;

namespace BloggingPlatformApi.DTOs;

public class PostDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new();
}