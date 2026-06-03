namespace groupProject.Infrastructure
{
  public interface IBudgetObserver
  {
    void Update(string message, decimal totalIncome, decimal remainingBudget);
  }
}