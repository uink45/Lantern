namespace Lantern.SSZ.Types.Bellatrix;

public class BellatrixState
{
    [SszElement(0, "uint64")] public ulong GenesisTime { get; set; }
    [SszElement(1, "Vector[uint8, 32]")] public byte[] GenesisValidatorsRoot { get; set; }
    [SszElement(2, "uint64")] public ulong Slot { get; set; }
    [SszElement(3, "Container")] public ForkSSZ Fork { get; set; }
    [SszElement(4, "Container")] public BeaconBlockHeaderSSZ LatestBlockHeader { get; set; }
    [SszElement(5, "Vector[Vector[uint8, 32], SLOTS_PER_HISTORICAL_ROOT]")] public byte[][] BlockRoots { get; set; }
    [SszElement(6, "Vector[Vector[uint8, 32], SLOTS_PER_HISTORICAL_ROOT]")] public byte[][] StateRoots { get; set; }
    [SszElement(7, "List[Vector[uint8, 32], HISTORICAL_ROOTS_LIMIT]")] public byte[][] HistoricalRoots { get; set; }
    [SszElement(8, "Container")] public Eth1DataSSZ Eth1Data { get; set; }
    [SszElement(9, "List[Container[Lantern.SSZ.Types.Eth1DataSSZ], ETH1_VOTE_DATA_LIMIT]")] public List<Eth1DataSSZ> Eth1DataVotes { get; set; }
    [SszElement(10, "uint64")] public ulong Eth1DepositIndex { get; set; }
    [SszElement(11, "List[Container[Lantern.SSZ.Types.ValidatorNodeStructSSZ], VALIDATOR_REGISTRY_LIMIT]")] public List<ValidatorNodeStructSSZ> Validators { get; set; }
    [SszElement(12, "List[uint64, VALIDATOR_REGISTRY_LIMIT]")] public List<ulong> Balances { get; set; }
    [SszElement(13, "Vector[Vector[uint8, 32], EPOCHS_PER_HISTORICAL_VECTOR]")] public byte[][] RandaoMixes { get; set; }
    [SszElement(14, "Vector[uint64, EPOCHS_PER_SLASHINGS_VECTOR]")] public ulong[] Slashings { get; set; }
    [SszElement(15, "List[uint8, VALIDATOR_REGISTRY_LIMIT]")] public byte[] PreviousEpochParticipation { get; set; }
    [SszElement(16, "List[uint8, VALIDATOR_REGISTRY_LIMIT]")] public byte[] CurrentEpochParticipation { get; set; }
    [SszElement(17, "Bitvector[JUSTIFICATION_BITS_LENGTH]")] public bool[] JustificationBits { get; set; }
    [SszElement(18, "Container")] public CheckpointSSZ PreviousJustifiedCheckpoint { get; set; }
    [SszElement(19, "Container")] public CheckpointSSZ CurrentJustifiedCheckpoint { get; set; }
    [SszElement(20, "Container")] public CheckpointSSZ FinalizedCheckpoint { get; set; }
    [SszElement(21, "List[uint64, VALIDATOR_REGISTRY_LIMIT]")] public ulong[] InactivityScores { get; set; }
    [SszElement(22, "Container")] public SyncCommitteeSSZ CurrentSyncCommittee { get; set; }
    [SszElement(23, "Container")] public SyncCommitteeSSZ NextSyncCommittee { get; set; }
    [SszElement(24, "Container")] public BellatrixExecutionPayloadHeader LastExecutionPayloadHeader { get; set; }
}