using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TasksToDo.DAL.iServices;
using TasksToDo.Models;

namespace TasksToDo.DAL.Services
{
    public class ToDoService : iToDoService
    {
        private readonly ApplicationDBContext _context;

        public ToDoService(ApplicationDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllTasks()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        public async Task<ToDoItem?> GetTaskByID(int TaskId)
        {
            var result = await _context.ToDoItems
                .FirstOrDefaultAsync(e => e.Id == TaskId);

            if (result != null)
                return result;

            return null;

        }

        public async Task<ToDoItem> AddTask(ToDoItem TaskItem)
        {
            var result = await _context.ToDoItems.AddAsync(TaskItem);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> UpdateTask(ToDoItem TaskItem)
        {
            var result = await _context.ToDoItems
                .FirstOrDefaultAsync(e => e.Id == TaskItem.Id);

            if (result != null)
            {
                result.Description = TaskItem.Description;
                result.IsCompleted = TaskItem.IsCompleted;
                result.CompletedDate= TaskItem.CompletedDate;
                result.CompletionComment = TaskItem.CompletionComment;  

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task DeleteTask(int TaskId)
        {
            try
            {

                var result = await _context.ToDoItems
                    .FirstOrDefaultAsync(e => e.Id == TaskId);
                if (result != null)
                {
                    _context.ToDoItems.Remove(result);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }


    }
}
