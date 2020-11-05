namespace YJC.Toolkit.Data
{
    public struct IndexField
    {
        public string FieldName { get; set; }

        public bool IsAscending { get; set; }

        public override int GetHashCode()
        {
            int strCode = FieldName == null ? 0 : FieldName.GetHashCode();
            return strCode ^ (IsAscending ? 1 : 0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IndexField))
                return false;
            return Equals((IndexField)obj);
        }

        public bool Equals(IndexField other)
        {
            return FieldName == other.FieldName && IsAscending == other.IsAscending;
        }

        public static bool operator ==(IndexField field1, IndexField field2)
        {
            return field1.Equals(field2);
        }

        public static bool operator !=(IndexField field1, IndexField field2)
        {
            return !field1.Equals(field2);
        }

        public static IndexField FromString(string fieldName)
        {
            return new IndexField() { FieldName = fieldName, IsAscending = true };
        }

        public static implicit operator IndexField(string fieldName)
        {
            return FromString(fieldName);
        }
    }
}
