﻿/*
 * Copyright 2017 Fireshark Studios, LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;

namespace InheritDocLib {
    public static class DictionaryX {

        public static V GetAs<T, U, V>(this Dictionary<T, U> me, T key, V defaultValue) {
            if (me.TryGetValue(key, out U value)) {
                return (V)Convert.ChangeType(value, typeof(V));
            }
            else {
                return defaultValue;
            }
        }

        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value) {
            key = tuple.Key;
            value = tuple.Value;
        }
    }
}
