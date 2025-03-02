
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Backend_Api.Data; 
using Backend_Api.Models; 

namespace Backend_Api.Controllers
{
    [Route("")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly TodoContext _context;

        public NotesController(TodoContext context)
        {
            _context = context;
        }

        // GET: /notes
        [HttpGet("notes")]
        public async Task<IActionResult> GetNotes([FromQuery] bool? completed)
        {
            var notes = await _context.Notes.ToListAsync();

            if (completed.HasValue)
            {
                notes = notes.Where(n => n.IsDone == completed.Value).ToList();
            }

            return Ok(notes);
        }

        // GET: /remaining
        [HttpGet("remaining")]
        public async Task<IActionResult> GetRemainingNotes()
        {
            var remaining = await _context.Notes.CountAsync(note => !note.IsDone);
            return Ok(remaining);
        }

        // POST: /notes
        [HttpPost("notes")]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            if (note == null || string.IsNullOrEmpty(note.Text))
            {
                return BadRequest("Note text is required.");
            }

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotes), note);
        }

        // PUT: /notes/{id}
        [HttpPut("notes/{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note updatedNote)
        {
            if (updatedNote == null || id != updatedNote.Id)
            {
                return BadRequest("Invalid note data.");
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            note.Text = updatedNote.Text;
            note.IsDone = updatedNote.IsDone;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: /notes/{id}
        [HttpDelete("notes/{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: /notes/toggle-all
        [HttpPost("notes/toggle-all")]
        public async Task<IActionResult> ToggleAll()
        {
            var allCompleted = await _context.Notes.AllAsync(note => note.IsDone);
            var notes = await _context.Notes.ToListAsync();

            foreach (var note in notes)
            {
                note.IsDone = !allCompleted;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: /clear-completed
        [HttpPost("clear-completed")]
        public async Task<IActionResult> ClearCompleted()
        {
            var completedNotes = await _context.Notes.Where(note => note.IsDone).ToListAsync();
            _context.Notes.RemoveRange(completedNotes);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}