using Lantern.Types.Containers;

namespace Lantern.SSZ.Consensus;

public class ForkSSZ
{
    [SszElement(0, "Vector[uint8, 4]")] public byte[] PreviousVersion { get; set; }
    [SszElement(1, "Vector[uint8, 4]")] public byte[] CurrentVersion { get; set; }
    [SszElement(2, "uint64")] public ulong Epoch { get; set; }
}

public class IndexedAttestationSSZ
{
    [SszElement(0, "List[uint64, MAX_VALIDATORS_PER_COMMITTEE]")] public bool[] AttestingIndices { get; set; }
    [SszElement(1, "Container")] public AttestationDataSSZ Data { get; set; }
    [SszElement(2, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
}

public class PendingAttestationSSZ
{
    [SszElement(0, "Bitlist[MAX_VALIDATORS_PER_COMMITTEE]")] public bool[] AttestingIndices { get; set; }
    [SszElement(1, "Container")] public AttestationDataSSZ Data { get; set; }
    [SszElement(2, "uint64")] public ulong InclusionDelay { get; set; }
    [SszElement(3, "uint64")] public ulong ProposerIndex { get; set; }
}

public class AttestationDataSSZ
{
    [SszElement(0, "uint64")] public ulong Slot { get; set; }
    [SszElement(1, "uint64")] public ulong CommitteeIndex { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] BeaconBlockRoot { get; set; }
    [SszElement(3, "Container")] public CheckpointSSZ Source { get; set; }
    [SszElement(4, "Container")] public CheckpointSSZ Target { get; set; }
}

public class AttestationSSZ
{
    [SszElement(0, "Bitlist[MAX_VALIDATORS_PER_COMMITTEE]")] public bool[] AggregationBits { get; set; }
    [SszElement(1, "Container")] public AttestationDataSSZ AttestationData { get; set; }
    [SszElement(2, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
}

public class DepositDataSSZ
{
    [SszElement(0, "Vector[uint8, 48]")] public byte[] Pubkey { get; set; }
    [SszElement(1, "Vector[uint8, 32]")] public byte[] WithdrawalCredentials { get; set; }
    [SszElement(2, "uint64")] public ulong Amount { get; set; }
    [SszElement(3, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
}

public class DepositSSZ
{
    [SszElement(0, "Vector[Vector[uint8, 32], DEPOSIT_PROOF_LENGTH]")] public byte[][] Proof { get; set; }
    [SszElement(1, "Container")] public DepositDataSSZ Data { get; set; }
}

public class SignedBeaconBlockHeaderSSZ
{
    [SszElement(0, "Container")] public BeaconBlockHeaderSSZ Header { get; set; }
    [SszElement(1, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
}

public class ProposerSlashingSSZ
{
    [SszElement(0, "Container")] public SignedBeaconBlockHeaderSSZ SignedHeader1 { get; set; }
    [SszElement(1, "Container")] public SignedBeaconBlockHeaderSSZ SignedHeader2 { get; set; }
}

public class AttesterSlashingSSZ
{
    [SszElement(0, "Container")] public IndexedAttestationSSZ Attestation1 { get; set; }
    [SszElement(1, "Container")] public IndexedAttestationSSZ Attestation2 { get; set; }
}

public class VoluntaryExitSSZ
{
    [SszElement(0, "uint64")] public ulong Epoch { get; set; }
    [SszElement(1, "uint64")] public ulong ValidatorIndex { get; set; }
}

public class SignedBeaconBlockSSZ
{
    [SszElement(0, "Container")] public BeaconBlockSSZ Block { get; set; }
    [SszElement(1, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
}

public class BeaconBlockSSZ
{
    [SszElement(0, "uint64")] public ulong Slot { get; set; }
    [SszElement(1, "uint64")] public ulong ValidatorIndex { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] ParentRoot { get; set; }
    [SszElement(3, "Vector[uint8, 32]")] public byte[] StateRoot { get; set; }
    [SszElement(4, "Container")] public BeaconBlockBodySSZ Body { get; set; }
}

public class SignedVoluntaryExitSSZ
{
    [SszElement(0, "Container")] public VoluntaryExitSSZ Exit { get; set; }
    [SszElement(1, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
}

public class BeaconBlockBodySSZ
{
    [SszElement(0, "Vector[uint8, 96]")] public byte[] RandaoReveal { get; set; }
    [SszElement(1, "Container")] public Eth1DataSSZ Eth1Data { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] Grafitti { get; set; }
    [SszElement(3, "List[Container, MAX_PROPOSER_SLASHINGS]")] public List<ProposerSlashingSSZ> ProposerSlashings { get; set; }
    [SszElement(4, "List[Container, MAX_ATTESTER_SLASHINGS]")] public List<AttesterSlashingSSZ> AttesterSlashings { get; set; }
    [SszElement(5, "List[Container, MAX_ATTESTATIONS]")] public List<AttestationSSZ> Attestations { get; set; }
    [SszElement(6, "List[Container, MAX_DEPOSITS]")] public List<DepositSSZ> Deposits { get; set; }
    [SszElement(7, "List[Container, MAX_VOLUNTARY_EXITS]")] public List<SignedVoluntaryExitSSZ> VoluntaryExits { get; set; }
    [SszElement(8, "Container")] public SyncAggregateSSZ SyncAggregate { get; set; }
    [SszElement(9, "Container")] public ExecutionPayload ExecutionPayload { get; set; }
}

public class Eth1DataSSZ
{
    [SszElement(0, "Vector[uint8, 32]")] public byte[] DepositRoot { get; set; }
    [SszElement(1, "uint64")] public ulong DepositCount { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] BlockHash { get; set; }
}

public class ValidatorNodeStructSSZ
{
    [SszElement(0, "Vector[uint8, 48]")] public byte[] Pubkey { get; set; }
    [SszElement(1, "Vector[uint8, 32]")] public byte[] WithdrawalCredentials { get; set; }
    [SszElement(2, "uint64")] public ulong EffectiveBalance { get; set; }
    [SszElement(3, "boolean")] public bool Slashed { get; set; }
    [SszElement(4, "uint64")] public ulong ActivationEligibilityEpoch { get; set; }
    [SszElement(5, "uint64")] public ulong ActivationEpoch { get; set; }
    [SszElement(6, "uint64")] public ulong ExitEpoch { get; set; }
    [SszElement(7, "uint64")] public ulong WithdrawableEpoch { get; set; }
}

public class CheckpointSSZ
{
    [SszElement(0, "uint64")] public ulong Epoch { get; set; }
    [SszElement(1, "Vector[uint8, 32]")] public byte[] Root { get; set; }
}

public class HistoricalSummarySSZ
{
    [SszElement(0, "Vector[uint8, 32]")] public byte[] BlockSummaryRoot { get; set; }
    [SszElement(1, "Vector[uint8, 32]")] public byte[] StateSummaryRoot { get; set; }
}

public class BeaconStateSSZ
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
    [SszElement(9, "List[Container[Lantern.SSZ.Consensus.Eth1DataSSZ], ETH1_VOTE_DATA_LIMIT]")] public List<Eth1DataSSZ> Eth1DataVotes { get; set; }
    [SszElement(10, "uint64")] public ulong Eth1DepositIndex { get; set; }
    [SszElement(11, "List[Container[Lantern.SSZ.Consensus.ValidatorNodeStructSSZ], VALIDATOR_REGISTRY_LIMIT]")] public List<ValidatorNodeStructSSZ> Validators { get; set; }
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
    [SszElement(24, "Container")] public ExecutionPayloadHeaderSSZ LastExecutionPayloadHeader { get; set; }
    [SszElement(25, "uint64")] public ulong NextWithdrawalIndex { get; set; }
    [SszElement(26, "uint64")] public ulong NextWithdrawalValidatorIndex { get; set; }
    [SszElement(27, "List[Container[Lantern.SSZ.Consensus.HistoricalSummarySSZ], HISTORICAL_ROOTS_LIMIT]")] public List<HistoricalSummarySSZ> HistoricalSummaries { get; set; }
}