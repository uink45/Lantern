using Lantern.Types.Containers;
using Lantern.Types.Crypto;
using Lantern.Config;
using Lantern.Core;
namespace Lantern.SSZ.Types.Capella;

public class CapellaBootstrap
{
    [SszElement(0, "Container")]
    public CapellaHeader Header { get; set; }

    [SszElement(1, "Container")]
    public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }

    [SszElement(2, "Vector[Vector[uint8, 32], CURRENT_SYNC_COMMITTEE_DEPTH]")]
    public byte[][] CurrentSyncCommitteeBranch { get; set; }

    public static CapellaBootstrap Serialize(LightClientBootstrap bootstrap)
    {
        CapellaBootstrap bootstrapSSZ = new CapellaBootstrap();
        bootstrapSSZ.Header = CapellaHeader.Serialize(bootstrap.Header);
        bootstrapSSZ.CurrentSyncCommittee = SyncCommitteeSSZ.Serialize(bootstrap.CurrentSyncCommittee);
        bootstrapSSZ.CurrentSyncCommitteeBranch = Enumerable.Range(0, Helpers.FloorLog2(Constants.CURRENT_SYNC_COMMITTEE_INDEX))
            .Select(i => bootstrapSSZ.CurrentSyncCommitteeBranch[i].AsSpan().ToArray())
            .ToArray();
        return bootstrapSSZ;
    }

    public static LightClientBootstrap? Deserialize(CapellaBootstrap bootstrapSSZ)
    {
        return new LightClientBootstrap(CapellaHeader.Deserialize(bootstrapSSZ.Header),
            SyncCommitteeSSZ.Deserialize(bootstrapSSZ.CurrentSyncCommittee),
            Enumerable.Range(0, Helpers.FloorLog2(Constants.CURRENT_SYNC_COMMITTEE_INDEX))
                .Select(i => new Bytes32(bootstrapSSZ.CurrentSyncCommitteeBranch[i]))
                .ToArray());
    }    
}