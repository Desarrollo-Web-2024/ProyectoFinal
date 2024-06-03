using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

public class EventDTO {
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
    public int ClientId { get; set; }
}

[ApiController]
[Route("event")]
public class EventController(Context.ProjectContext projectContext) : ControllerBase{
    private DbSet<Event> Container { get; } = projectContext.Events;
    
    [HttpPost]
    public async Task<ActionResult<Event>> Post(EventDTO uploadedEvent) {
        var client = await projectContext.Users.FindAsync(uploadedEvent.ClientId);
        if (client == null) {
            return BadRequest();
        }
        
        var newEvent = new Event() {
            Client = client,
            Description = uploadedEvent.Description,
            Name = uploadedEvent.Name,
            StartDate = uploadedEvent.StartDate,
            Duration = uploadedEvent.Duration,
            Solved = false,
            Id = 0
        };
        
        await Container.AddAsync(newEvent);
        await projectContext.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new {id= newEvent.Id}, newEvent);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Event>> Get(int id) {
        var rEvent = await Container.Include(e => e.Client).Where(e => e.Id == id).SingleOrDefaultAsync();
        if (rEvent == null) {
            return NotFound();
        }
        
        return rEvent;
    }
    [HttpGet("get-unsolved")]
    public async Task<ActionResult<List<Event>>> GetUnsolved(int userId) {
        var events = await Container.Include(e => e.Client).Where(e =>  e.Client.Id == userId && !e.Solved).ToListAsync();
        return events;
    }
    
    [HttpGet("get-solved")]
    public async Task<ActionResult<List<Event>>> GetSolved(int userId) {
        var events = await Container.Include(e => e.Client).Where(e => e.Client.Id == userId && e.Solved).ToListAsync();
        return events;
    }
    
    [HttpGet("get-unsolved-all")]
    public async Task<ActionResult<List<Event>>> GetUnsolvedAll() {
        var events = await Container.Include(e => e.Client).Where(e => !e.Solved).ToListAsync();
        return events;
    }
    
    [HttpGet("get-solved-all")]
    public async Task<ActionResult<List<Event>>> GetSolvedAll() {
        var events = await Container.Include(e => e.Client).Where(e => e.Solved).ToListAsync();
        return events;
    }
}