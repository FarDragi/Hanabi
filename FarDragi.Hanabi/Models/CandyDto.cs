namespace FarDragi.Hanabi.Models;

public record CandyDto(ulong Id, int Count)
{
    public static implicit operator CandyEntity(CandyDto dto)
    {
        return new CandyEntity(dto.Id, dto.Count);
    }

    public static implicit operator CandyDto(CandyEntity entity)
    {
        return new CandyDto(entity.Id, entity.Count);
    }
}