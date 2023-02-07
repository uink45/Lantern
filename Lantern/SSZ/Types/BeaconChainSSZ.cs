using Lantern.Types.Containers;

namespace Lantern.SSZ.Types;

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

public class SignedVoluntaryExitSSZ
{
    [SszElement(0, "Container")] public VoluntaryExitSSZ Exit { get; set; }
    [SszElement(1, "Vector[uint8, 96]")] public byte[] Signature { get; set; }
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

public class BlsToExecutionChange
{
    [SszElement(0, "uint64")] public ulong ValidatorIndex { get; set; }
    [SszElement(1, "Vector[uint8, 48]")] public byte[] FromBlsPubkey { get; set; }
    [SszElement(2, "Vector[uint8, 20]")] public byte[] ToExecutionAddress { get; set; }
}

public class SignedBLSToExecutionChange
{
    [SszElement(0, "Container")] public BlsToExecutionChange Message { get; set; }
    [SszElement(1, "Vector[uint8, 96]")] public byte[] Signature{ get; set; }
}

public class Withdrawal
{
    [SszElement(0, "uint64")] public ulong Index { get; set; }
    [SszElement(1, "uint64")] public ulong ValidatorIndex { get; set; }
    [SszElement(2, "Vector[uint8, 20]")] public byte[] Address { get; set; }
    [SszElement(3, "uint64")] public ulong Amount { get; set; }
}