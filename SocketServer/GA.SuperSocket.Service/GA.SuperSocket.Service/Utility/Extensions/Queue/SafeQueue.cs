using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Globalegrow.Toolkit
{
    /// <summary>
    /// 参考:https://msdn.microsoft.com/en-us/library/de0542zz(v=vs.110).aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeQueue<T>
    {
        // A queue that is protected by Monitor. 
        private Queue<T> m_inputQueue = new Queue<T>();

        // Lock the queue and add an element. 
        public void Enqueue(T qValue)
        {
            // Request the lock, and block until it is obtained.
            Monitor.Enter(m_inputQueue);
            try
            {
                // When the lock is obtained, add an element.
                m_inputQueue.Enqueue(qValue);
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(m_inputQueue);
            }
        }

        // Try to add an element to the queue: Add the element to the queue  
        // only if the lock is immediately available. 
        public bool TryEnqueue(T qValue)
        {
            // Request the lock. 
            if (Monitor.TryEnter(m_inputQueue))
            {
                try
                {
                    m_inputQueue.Enqueue(qValue);
                }
                finally
                {
                    // Ensure that the lock is released.
                    Monitor.Exit(m_inputQueue);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Try to add an element to the queue: Add the element to the queue  
        // only if the lock becomes available during the specified time 
        // interval. 
        public bool TryEnqueue(T qValue, int waitTime)
        {
            // Request the lock. 
            if (Monitor.TryEnter(m_inputQueue, waitTime))
            {
                try
                {
                    m_inputQueue.Enqueue(qValue);
                }
                finally
                {
                    // Ensure that the lock is released.
                    Monitor.Exit(m_inputQueue);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Lock the queue and dequeue an element. 
        public T Dequeue()
        {
            T retval;

            // Request the lock, and block until it is obtained.
            Monitor.Enter(m_inputQueue);
            try
            {
                // When the lock is obtained, dequeue an element.
                retval = m_inputQueue.Dequeue();
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(m_inputQueue);
            }

            return retval;
        }

        // Delete all elements that equal the given object. 
        public int Remove(T qValue)
        {
            int removedCt = 0;

            // Wait until the lock is available and lock the queue.
            Monitor.Enter(m_inputQueue);
            try
            {
                int counter = m_inputQueue.Count;
                while (counter > 0)
                // Check each element.
                {
                    T elem = m_inputQueue.Dequeue();
                    if (!elem.Equals(qValue))
                    {
                        m_inputQueue.Enqueue(elem);
                    }
                    else
                    {
                        // Keep a count of items removed.
                        removedCt += 1;
                    }
                    counter = counter - 1;
                }
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(m_inputQueue);
            }

            return removedCt;
        }

        // Print all queue elements. 
        public string PrintAllElements()
        {
            StringBuilder output = new StringBuilder();

            // Lock the queue.
            Monitor.Enter(m_inputQueue);
            try
            {
                foreach (T elem in m_inputQueue)
                {
                    // Print the next element.
                    output.AppendLine(elem.ToString());
                }
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(m_inputQueue);
            }

            return output.ToString();
        }

        public int Count
        {
            get
            {
                int count;
                // Lock the queue.
                Monitor.Enter(m_inputQueue);
                try
                {
                    count = m_inputQueue.Count();
                }
                finally
                {
                    // Ensure that the lock is released.
                    Monitor.Exit(m_inputQueue);
                }
                return count;
            }
        }

        public bool Contains(T item)
        {
            // Lock the queue.
            Monitor.Enter(m_inputQueue);
            try
            {
                return m_inputQueue.Contains(item);
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(m_inputQueue);
            }
        }

        public bool Contains(T item, IEqualityComparer<T> comparer)
        {
            // Lock the queue.
            Monitor.Enter(m_inputQueue);
            try
            {
                return m_inputQueue.Contains(item, comparer);
            }
            finally
            {
                // Ensure that the lock is released.
                Monitor.Exit(m_inputQueue);
            }
        }

    }
}
