using System.Numerics;
using YamlDotNet.Serialization;

namespace Lantern.Tests.Models;

public class ConfigData
{
    public string GENESIS_FORK_VERSION { get; set; }
    
    public string? ALTAIR_FORK_VERSION { get; set; }
    
    public string? ALTAIR_FORK_EPOCH { get; set; }
    
    public string? BELLATRIX_FORK_VERSION { get; set; }
    
    public string? BELLATRIX_FORK_EPOCH { get; set; }
    
    public string? CAPELLA_FORK_VERSION { get; set; }
    
    public string? CAPELLA_FORK_EPOCH { get; set; }
    
    public string? EIP4844_FORK_VERSION { get; set; }
    
    public string? EIP4844_FORK_EPOCH { get; set; }
    
    public string PRESET_BASE { get; set; }
    
    public string MIN_GENESIS_ACTIVE_VALIDATOR_COUNT { get; set; }
    
    public string MIN_GENESIS_TIME { get; set; }
    
    public string GENESIS_DELAY { get; set; }
    
    public string SECONDS_PER_SLOT { get; set; }
    
    public string SECONDS_PER_ETH1_BLOCK { get; set; }
    
    public string MIN_VALIDATOR_WITHDRAWABILITY_DELAY { get; set; }
    
    public string SHARD_COMMITTEE_PERIOD { get; set; }
    
    public string ETH1_FOLLOW_DISTANCE { get; set; }
    
    public string EJECTION_BALANCE { get; set; }
    
    public string MIN_PER_EPOCH_CHURN_LIMIT { get; set; }
    
    public string CHURN_LIMIT_QUOTIENT { get; set; }
    
    public string PROPOSER_SCORE_BOOST { get; set; }
    
    public string INACTIVITY_SCORE_BIAS { get; set; }
    
    public string INACTIVITY_SCORE_RECOVERY_RATE { get; set; }
    
    public string TERMINAL_TOTAL_DIFFICULTY { get; set; }
    
    public string TERMINAL_BLOCK_HASH { get; set; }
    
    public string TERMINAL_BLOCK_HASH_ACTIVATION_EPOCH { get; set; }
}