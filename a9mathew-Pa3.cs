// -------------------------------------------------------------------
// Department of Electrical and Computer Engineering
// University of Waterloo
//
// Student Name:     Anand Mathew
// Userid:           a9mathew
//
// Assignment:       PA3
// Submission Date:  2014-14-11
// 
// I declare that, other than the acknowledgements listed below, 
// this program is my original work.
//
// Acknowledgement:
// 
// -------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Class holds the sequence description of a protein.
class Protein
{
    string id;
    string name;
    string sequence;
    
    public Protein( string id, string name, string sequence )
    {
        this.id = id;
        this.name = name;
        this.sequence = sequence;
    }
    
    public string Id  get { return id; } }
    public string Name { get { return name; } }
    
    // Does the protein sequence contain a specified subsequence?
    // Method returns true if this protein's sequence contains the substring passed
    // as a parameter, and false otherwise.
    
    public bool ContainsSubsequence( string sub )
    {
        if (sub!=null)
        {
            if (sequence.Contains( sub ))
            {
                return true;
            }
            return false;
        }
        else
        {
            throw new Exception ("Strings cannot be null.");
        }
    }
    
    // How often does a specified subsequence occur in the protein?
    // Method returns the count of the number of times the 
    // substring passed as a parameter appears in this protein's sequence.
    
    public int CountSubsequence( string sub )
    {
        if (sub!=null)
        {
            int countOccurence=0;
            
            for (int i=0; i<sequence.Length; i++)
            {
                if(i+sub.Length<=sequence.Length)
                {
                    if(sequence.Substring(i,sub.Length).ToLower()
                                          .Equals(sub.ToLower()))
                    {
                        countOccurence++;
                    }
                }
            
            }
            return countOccurence;
        }
        else
        {
            throw new Exception ("Strings cannot be null.");
        }
    }   
    
    // Method returns a string the same length as this protein's 
    // sequence but with every character set to '.' except 
    // for places where the substring passed as a parameter 
    // appears in this protein's sequence.
    
    public string LocateSubsequence(string sub)
    {
       if (sub!=null)
       {
           string newString = "";
           if (sequence.Contains(sub))
            {
                newString = sequence.Replace(sub,"*");
                for (int i = 0; i<newString.Length; i++)
                {
                    if (newString[i] != '*')
                    {
                        newString = newString.Replace(newString[i],'.');
                    }
                   
                }
            }
            newString = newString.Replace("*", sub);
            return newString;
        }
        else 
        {
            throw new Exception ("Strings cannot be null.");
        }
        
    }
    
    
    // Method writes this protein's information, in FASTA form, 
    // to the TextWriter stream passed as a paramter.
    public void WriteFasta( TextWriter output )
    {
        if (output!=null)
        {
            output.WriteLine();
            output.Write(">" + id + name);
            
            for (int i =0; i< sequence.Length; i++)
            {
                if (i % 80 == 0 && i!= 0)
                {
                    output.WriteLine();+
                }
                else
                {
                    output.Write(sequence[i]);
                }
            }
            return;
        }
        else 
        {
            throw new Exception ("Strings cannot be null.");
        }
    }
}

// Read a protein file into a collection and test the functionality of
// methods in the Protein class.
static class Program
{
    static string fastaFile = "protein.fasta";
    
    //Used ArrayList to hold all the proteins in the file.
    
    static ArrayList proteins = new ArrayList( );         
    
    static void Main( )
    {
        
        // Reads the protein.fasta file,
        // collects the header and sequence lines 
        // for one protein, forms the new Protein object, and 
        // adds it to the ArrayList. 
        using( StreamReader sr = new StreamReader( fastaFile ) )
        {
            string line = sr.ReadLine( );
            string header=null;
            string sequence=null;
            bool finishedSequence=false;
            
            while( line != null)
            {            
                header=null;
                while (header==null && line!=null)
                {           
                    if(line!=null)
                    {
                        if(line.Equals(""))
                        {
                            line = sr.ReadLine( );
                        }
                        else if(line.StartsWith(">"))
                        {
                            header=line;
                        }
                        else
                        {
                            throw new Exception ("Expected a header line");
                        }         
                    }
                }
                
                sequence=null;
                finishedSequence=false;
                while(finishedSequence==false && line!=null)
                {
                    line = sr.ReadLine( );
                    if(line!=null)
                    {
                        if(!line.StartsWith(">"))
                        {
                            sequence=sequence+line;
                        }
                        else if(line.StartsWith(">"))
                        {
                            if (!(sequence==null))
                            {
                                finishedSequence=true;
                            }
                        }
                        else
                        {
                            throw new Exception ("Expected a sequence line");
                        }
                    }
                }
                
                if(!(header==null))
                {
                    if(!(sequence==null))
                    {
                        Protein Protein = new Protein (
                                                       header.Substring (1,11),
                                                       header.Substring (12,
                                                       (header.Length-12)), 
                                                       sequence) ;
                        proteins.Add(Protein);
                    }
                    else
                    {
                        throw new Exception ("Header with Missing Sequence");
                    }
                }
            }
        }
        
        
        
        // Report count of proteins loaded.
        Console.WriteLine( );
        Console.WriteLine( "Loaded {0} proteins from the {1} file.", 
            proteins.Count, fastaFile );
          
        // Report proteins containing a specified sequence.
        Console.WriteLine( );
        Console.WriteLine( "Proteins containing sequence RILED:" );
        foreach( Protein p in proteins )
        {
            if( p.ContainsSubsequence( "RILED" ) )
            {
                Console.WriteLine( p.Name );
            }
        }
        
        // Report proteins containing a repeated sequence.
        Console.WriteLine( );
        Console.WriteLine( 
            "Proteins containing sequence SNL more than 5 times:" );
        foreach( Protein p in proteins )
        {
            if( p.CountSubsequence( "SNL" ) > 5 )
            {
                Console.WriteLine( p.Name );
            }
        }
        
        // Locate the specified sequence in proteins containing it.
        Console.WriteLine( );
        Console.WriteLine( "Proteins containing sequence DEVGG:" );
        foreach( Protein p in proteins )
        {
            if( p.ContainsSubsequence( "DEVGG" ) )
            {
                Console.WriteLine( p.Name );
                Console.WriteLine( p.LocateSubsequence( "DEVGG" ) );
            }
        }
        
        // Show FASTA output for proteins containing a specified sequence.
        Console.WriteLine( );
        Console.WriteLine( "Proteins containing sequence DEVGG:" );
        foreach( Protein p in proteins )
        {
            if( p.ContainsSubsequence( "DEVGG" ) )
            {
                p.WriteFasta( Console.Out );
            }
        }
        
    }
}