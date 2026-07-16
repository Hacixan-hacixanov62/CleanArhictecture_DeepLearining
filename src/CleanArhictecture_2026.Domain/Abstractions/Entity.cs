namespace CleanArhictecture_2026.Domain.Abstractions;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    #region Audit Log
    public DateTime CreateAt { get; set; } //DateTimeOffset
                                                 //niye DateTimeOffset kullandık ?
                                                 //Cunki basqa olkede olanda saat ferqine gore vaxt ferqi olacaq ve bu ferqi DateTimeOffset ile asanliqla hesablayacagik

    public Guid CreateUserId { get; set; } = default!;
    public DateTime? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion

}