using Core.Domain;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Panels;

[Mapper]
public static partial class PanelMappers
{
    [MapperIgnoreSource(nameof(Panel.MainSerialNumber))]
    public static partial PanelDbModel ToDbModel(this Panel panel);

    public static partial Panel ToDomainModel(this PanelDbModel panelDbModel);
}