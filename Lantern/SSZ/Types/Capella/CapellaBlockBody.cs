namespace Lantern.SSZ.Types.Capella;

public class CapellaBlockBody
{
    [SszElement(0, "Vector[uint8, 96]")] public byte[] RandaoReveal { get; set; }
    [SszElement(1, "Container")] public Eth1DataSSZ Eth1Data { get; set; }
    [SszElement(2, "Vector[uint8, 32]")] public byte[] Grafitti { get; set; }

    [SszElement(3, "List[Container, MAX_PROPOSER_SLASHINGS]")]
    public List<ProposerSlashingSSZ> ProposerSlashings { get; set; }

    [SszElement(4, "List[Container, MAX_ATTESTER_SLASHINGS]")]
    public List<AttesterSlashingSSZ> AttesterSlashings { get; set; }

    [SszElement(5, "List[Container, MAX_ATTESTATIONS]")]
    public List<AttestationSSZ> Attestations { get; set; }

    [SszElement(6, "List[Container, MAX_DEPOSITS]")]
    public List<DepositSSZ> Deposits { get; set; }

    [SszElement(7, "List[Container, MAX_VOLUNTARY_EXITS]")]
    public List<SignedVoluntaryExitSSZ> VoluntaryExits { get; set; }

    [SszElement(8, "Container")] public SyncAggregateSSZ SyncAggregate { get; set; }
    [SszElement(9, "Container")] public CapellaExecutionPayload ExecutionPayload { get; set; }

    [SszElement(10, "List[Container, MAX_BLS_TO_EXECUTION_CHANGES]")]
    public List<SignedBLSToExecutionChange> BlsToExecutionChanges { get; set; }
}