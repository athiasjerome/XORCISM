using System;
using System.Collections.Generic;
using System.Text;

// http://nvd.nist.gov/cvsseq2.htm

namespace Import_all
{
    class CVSSCalculator
    {
        // ======================
        // Exploitability Metrics
        // ======================

        public enum AccessComplexity
        {
            High,
            Medium,
            Low
        };

        public enum Authentication
        {
            None,
            SingleInstance,
            MultipleIntances
        };

        public enum AccessVector
        {
            Local,
            LocalNetwork,
            Network
        };

        // ==============
        // Impact Metrics
        // ==============

        public enum ConfidentialityImpact
        {
            None,
            Partial,
            Complete
        };
 
        public enum IntegrityImpact
        {
            None,
            Partial,
            Complete
        };

        public enum AvailabilityImpact
        {
            None,
            Partial,
            Complete
        };


        Dictionary<AccessComplexity, double>    m_DicoAccessComplexity;
        Dictionary<Authentication, double>      m_DicoAuthentication;
        Dictionary<AccessVector, double>        m_DicoAccessVector;

        Dictionary<ConfidentialityImpact, double>  m_DicoConfidentialityImpact;
        Dictionary<IntegrityImpact, double>        m_DicoIntegrityImpact;
        Dictionary<AvailabilityImpact, double>      m_DicoAvailabilityImpact;
        
        

        public CVSSCalculator()
        {
            m_DicoAccessComplexity = new Dictionary<AccessComplexity,double>();
            m_DicoAccessComplexity.Add(AccessComplexity.High, 0.35);
            m_DicoAccessComplexity.Add(AccessComplexity.Medium, 0.61);
            m_DicoAccessComplexity.Add(AccessComplexity.Low, 0.71);

            m_DicoAuthentication = new Dictionary<Authentication, double>();
            m_DicoAuthentication.Add(Authentication.None, 0.704);
            m_DicoAuthentication.Add(Authentication.SingleInstance, 0.56);
            m_DicoAuthentication.Add(Authentication.MultipleIntances, 0.45);

            m_DicoAccessVector = new Dictionary<AccessVector, double>();
            m_DicoAccessVector.Add(AccessVector.Local, 0.395);
            m_DicoAccessVector.Add(AccessVector.LocalNetwork, 0.646);
            m_DicoAccessVector.Add(AccessVector.Network, 1);

            m_DicoConfidentialityImpact = new Dictionary<ConfidentialityImpact,double>();
            m_DicoConfidentialityImpact.Add(ConfidentialityImpact.None, 0);
            m_DicoConfidentialityImpact.Add(ConfidentialityImpact.Partial, 0.275);
            m_DicoConfidentialityImpact.Add(ConfidentialityImpact.Complete, 0.660);

            m_DicoIntegrityImpact = new Dictionary<IntegrityImpact,double>();
            m_DicoIntegrityImpact.Add(IntegrityImpact.None, 0);
            m_DicoIntegrityImpact.Add(IntegrityImpact.Partial, 0.275);
            m_DicoIntegrityImpact.Add(IntegrityImpact.Complete, 0.660);

            m_DicoAvailabilityImpact = new Dictionary<AvailabilityImpact,double>(); 
            m_DicoAvailabilityImpact.Add(AvailabilityImpact.None, 0);
            m_DicoAvailabilityImpact.Add(AvailabilityImpact.Partial, 0.275);
            m_DicoAvailabilityImpact.Add(AvailabilityImpact.Complete, 0.660);


        }

        public void Calculate(AccessComplexity accessComplexity, Authentication authentication, AccessVector accessVector, ConfidentialityImpact confidentialityImpact, IntegrityImpact integrityImpact, AvailabilityImpact availabilityImpact, out double baseScore, out double impactSubscore, out double exploitabilitySubscore)
        {
            // ==============
            // ImpactSubscore
            // ==============

            double ConfImpact = m_DicoConfidentialityImpact[confidentialityImpact];
            double IntegImpact = m_DicoIntegrityImpact[integrityImpact];
            double AvailImpact = m_DicoAvailabilityImpact[availabilityImpact];

            impactSubscore = 10.41 * (1.0 - (1.0 - ConfImpact)*(1.0 - IntegImpact) * (1.0 - AvailImpact));

            // ======================
            // ExploitabilitySubscore
            // ======================

            double accessComplexityDouble = m_DicoAccessComplexity[accessComplexity];
            double authenticationDouble = m_DicoAuthentication[authentication];
            double accessVectorDouble = m_DicoAccessVector[accessVector];

            exploitabilitySubscore = 20.0 * accessComplexityDouble * authenticationDouble * accessVectorDouble;

            // =========
            // BaseScore
            // =========

            double fImpact;
            fImpact = impactSubscore == 0.0 ? 0.0 : 1.176;

            baseScore = (0.6 * impactSubscore + 0.4 * exploitabilitySubscore - 1.5) * fImpact;
        }
    }
}
