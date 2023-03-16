using TasksToDo.Models;

namespace TasksToDo.DAL.iServices
{
    public interface iToDoService
    {
        Task<IEnumerable<ToDoItem>> GetAllTasks();
        Task<ToDoItem> GetTaskByID(int TaskId);
        Task<ToDoItem> AddTask(ToDoItem TaskItem);
        Task<bool> UpdateTask(ToDoItem TaskItem);
        void DeleteTask(int TaskId);
        bool ItemExists(int id);
    }
}
