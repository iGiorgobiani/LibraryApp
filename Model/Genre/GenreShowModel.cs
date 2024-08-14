

namespace Model.Genre;

public class GenreShowModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? Total {  get; set; }

    public IPagedList<GenreListItem> Genres { get; set; }
}
