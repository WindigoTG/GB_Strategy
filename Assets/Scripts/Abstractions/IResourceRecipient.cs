public interface IResourceRecipient
{
    int FactionID { get; }
    void DepositResources((ResourceType resourceType, int resourceAmount) resourceLoad);
}
