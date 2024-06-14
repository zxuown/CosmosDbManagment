namespace CosmosDbManagment.Models;

public class TaskIndexViewModel
{
    public List<Task> Tasks { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
