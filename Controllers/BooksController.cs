using ApiBooks.Data;
using ApiBooks.Models; 
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;

        public BooksController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _bookRepository.GetAll();
            return Ok(books);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound();
        }
        
        [HttpPost("digital")]
        public IActionResult AddDigitalBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            _bookRepository.Add(book);

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        
        [HttpPut("digital/{id}")]
        public IActionResult UpdateDigitalBook(int id, [FromBody] Book book)
        {
            if (book == null || book.Id != id)
            {
                return BadRequest();
            }

            _bookRepository.Update(book);

            return NoContent();
        }
        
        [HttpDelete("digital/{id}")]
        public IActionResult DeleteDigitalBook(int id)
        {
            _bookRepository.Delete(id);

            return NoContent();
        }
    }
}