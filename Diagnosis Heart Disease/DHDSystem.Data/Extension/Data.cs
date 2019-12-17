using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace DHDSystem.Data.Extension
{
    public class Data
    {
        private IGraph _graph;
        private SparqlQueryParser _parser;
        public Data()
        {
            this._graph = new VDS.RDF.Graph();
            var file = @"C:\study\Diagnosis Heart Disease\Diagnosis Heart Disease\Test\example.owl";
            _graph.LoadFromFile(file);
            this._parser = new SparqlQueryParser();
        }
        private SparqlParameterizedString AddNamespace()
        {
            SparqlParameterizedString query = new SparqlParameterizedString();
            query.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            query.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            query.Namespaces.AddNamespace("uni", new Uri("http://www.semanticweb.org/dserious/ontologies/2019/11/HeartOnt_uit#"));
            return query;
         }
        public List<string> GetAllSymptoms()
        {
            List<string> symptoms = new List<string>();
            SparqlParameterizedString query = AddNamespace();

            query.CommandText = "SELECT ?symptom WHERE {?symptom rdf:type uni:Symptoms}";
            SparqlQuery queryEx = _parser.ParseFromString(query);
            Object result = _graph.ExecuteQuery(queryEx);
            if (result is SparqlResultSet)
            {
                String symptomLabel;
                SparqlResultSet rset = (SparqlResultSet)result;
                foreach (SparqlResult r in rset)
                {
                    INode n;
                    if (r.TryGetValue("symptom", out n))
                    {
                        symptomLabel = ((IUriNode)n).Uri.Fragment.Replace("#", "");
                        symptoms.Add(symptomLabel);
                    }
                }
            }
            return symptoms;
        }
        public List<string> GetDiseases(List<string> symptoms)
        {
            List<string> diseases = new List<string>();
            SparqlParameterizedString query = AddNamespace();
            string q = "SELECT ?diseases WHERE {";
            
            foreach (var symptom in symptoms)
            {

                string temp = $"?diseases uni:hasSymptom uni:{symptom}.";
                q = q + temp;
            }
            q = q + "}";
            query.CommandText = q;

            SparqlQuery queryEx = _parser.ParseFromString(query);
            Object result = _graph.ExecuteQuery(queryEx);
            if (result is SparqlResultSet)
            {
                String diseaseLabel;
                SparqlResultSet rset = (SparqlResultSet)result;
                foreach (SparqlResult r in rset)
                {
                    INode n;
                    if (r.TryGetValue("diseases", out n))
                    {
                        diseaseLabel = ((IUriNode)n).Uri.Fragment.Replace("#", "");
                        diseases.Add(diseaseLabel);
                    }
                }
            }
            return diseases;
        }
        public  List<string> GetTreatmentFor(string disease)
        {
            List<string> treatments =new  List<string>();

            SparqlParameterizedString query = AddNamespace();
            string q = "SELECT ?treatment WHERE {?treatment uni:treatmentFor uni:" + disease+"}";
            query.CommandText = q;

            SparqlQuery queryEx = _parser.ParseFromString(query);
            Object result = _graph.ExecuteQuery(queryEx);
            if (result is SparqlResultSet)
            {
                String treatmentLabel;
                SparqlResultSet rset = (SparqlResultSet)result;
                foreach (SparqlResult r in rset)
                {
                    INode n;
                    if (r.TryGetValue("treatment", out n))
                    {
                        treatmentLabel = ((IUriNode)n).Uri.Fragment.Replace("#", "");
                        treatments.Add(treatmentLabel);
                    }
                }
            }
            return treatments;
        }

    }
}
