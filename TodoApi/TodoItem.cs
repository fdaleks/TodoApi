﻿namespace TodoApi;

public class TodoItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsCompleted{ get; set; }
}
