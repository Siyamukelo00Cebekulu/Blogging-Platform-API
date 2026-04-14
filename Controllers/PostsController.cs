using BloggingPlatformApi.DTOs;
using BloggingPlatformApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatformApi.Controllers;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(PostDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            Category = dto.Category,
            Tags = dto.Tags,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PostDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _context.Posts.FindAsync(id);
        if (post == null)
            return NotFound();

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.Category = dto.Category;
        post.Tags = dto.Tags;
        post.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(post);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
            return NotFound();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET SINGLE
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
            return NotFound();

        return Ok(post);
    }

    // GET ALL + SEARCH
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? term)
    {
        var query = _context.Posts.AsQueryable();

        if (!string.IsNullOrEmpty(term))
        {
            term = term.ToLower();

            query = query.Where(p =>
                p.Title.ToLower().Contains(term) ||
                p.Content.ToLower().Contains(term) ||
                p.Category.ToLower().Contains(term)
            );
        }

        var posts = await query.ToListAsync();
        return Ok(posts);
    }
}