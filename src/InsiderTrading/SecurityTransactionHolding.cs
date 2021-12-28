using System;

namespace Aletheia.Service.InsiderTrading
{
    public class SecurityTransactionHolding
    {
        //Standard data - always there regardless of whether or not it is a transaction or holding!
        public Guid Id {get; set;}
        public Guid FromFiling {get; set;}
        public SecFiling _FromFiling {get; set;} //For the class library only
        public TransactionHoldingEntryType EntryType {get; set;}
        public float QuantityOwnedFollowingTransaction {get; set;}
        public DirectIndirect DirectIndirect {get; set;}
        public string SecurityTitle {get; set;}
        public SecurityType SecurityType {get; set;}

        //Transaction-Related!
        public AcquiredDisposed? AcquiredDisposed {get; set;}
        public float? Quantity {get; set;}
        public float? PricePerSecurity {get; set;}
        public DateTime? TransactionDate {get; set;}
        public TransactionCode? TransactionCode {get; set;}
        
        //Derivative-specific data
        public float? ConversionOrExercisePrice {get; set;}
        public DateTime? ExercisableDate {get; set;}
        public DateTime? ExpirationDate {get; set;}
        public string UnderlyingSecurityTitle {get; set;}
        public float? UnderlyingSecurityQuantity {get; set;}
    }
}