using UnityEngine;

public class AlgorithmPlayer : MonoBehaviour
{
    private double risk;
    [SerializeField] private double riskFactor;
    [SerializeField] private double foldLimit;
    [SerializeField] private double callLimit;
    [SerializeField] private double raiseLimit;
    [SerializeField] private double allinLimit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void calculateRisk(double pot, double bet, double stack)
    {

    }
    void updateRisk(double pot, double bet, double stack)
    {
        risk = (pot + bet) / stack;
    }
}
