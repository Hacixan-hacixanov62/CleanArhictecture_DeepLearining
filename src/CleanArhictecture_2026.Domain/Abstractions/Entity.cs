namespace CleanArhictecture_2026.Domain.Abstractions;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; } //DateTimeOffset
                                                 //niye DateTimeOffset kullandık ?
                                                 //Cunki basqa olkede olanda saat ferqine gore vaxt ferqi olacaq ve bu ferqi DateTimeOffset ile asanliqla hesablayacagik
    public DateTime? UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }

}